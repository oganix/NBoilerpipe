using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NBoilerpipe.Parser;
using NBoilerpipe.Document;
using NBoilerpipe.Labels;
using NBoilerpipe.Util;
using Sharpen;

namespace NBoilerpipe
{

    public class NBoilerpipeContentHandler : IContentHandler
    {
		enum Event
		{
			START_TAG,
			END_TAG,
			CHARACTERS,
			WHITESPACE
		}
		
		readonly IDictionary<string, TagAction> tagActions = DefaultTagActionMap.INSTANCE;
		string title = null;

		internal static readonly string ANCHOR_TEXT_START = "$\ue00a";
		internal static readonly string ANCHOR_TEXT_END = "\ue00a$";
		internal StringBuilder tokenBuilder = new StringBuilder();
		internal StringBuilder textBuilder = new StringBuilder();
		internal int inBody = 0;
		internal int inAnchor = 0;
		internal int inIgnorableElement = 0;
		internal int tagLevel = 0;
		internal int blockTagLevel = -1;
		internal bool sbLastWasWhitespace = false;

		int textElementIdx = 0;
		readonly IList<TextBlock> textBlocks = new AList<TextBlock>();
		string lastStartTag = null;
		string lastEndTag = null;
		NBoilerpipeContentHandler.Event lastEvent;
		int offsetBlocks = 0;
		BitSet currentContainedTextElements = new BitSet();
		bool flush = false;
		bool inAnchorText = false;
		internal List<List<LabelAction>> labelStacks = new List<List<LabelAction>>();
		internal List<int?> fontSizeStack = new List<int?>();
		
		static readonly Sharpen.Pattern PAT_VALID_WORD_CHARACTER = Sharpen.Pattern
			.Compile ("[\\p{L}\\p{Nd}\\p{Nl}\\p{No}]");
		
		
		public void StartElement (HtmlNode node)
		{
			labelStacks.AddItem (null);
			TagAction ta = tagActions.Get (node.Name);
			if (ta != null) {
				if (ta.ChangesTagLevel ()) {
					tagLevel++;
				}
				flush = ta.Start (this, node.Name, node.Attributes) | flush;
			} else {
				tagLevel++;
				flush = true;
			}
			lastEvent = NBoilerpipeContentHandler.Event.START_TAG;
			lastStartTag = node.Name;
		}
		
		public void EndElement (HtmlNode node)
		{
			TagAction ta = tagActions.Get (node.Name);
			if (ta != null) {
				flush = ta.End (this, node.Name) | flush;
			} else {
				flush = true;
			}
			if (ta == null || ta.ChangesTagLevel ()) {
				tagLevel--;
			}
			if (flush) {
				FlushBlock ();
			}
			lastEvent = NBoilerpipeContentHandler.Event.END_TAG;
			lastEndTag = node.Name;
			labelStacks.RemoveLast ();
		}
		
        public void HandleText (HtmlTextNode node)
		{
			if (IsTag (node.Text))
				node.Text = "";
			
			char[] ch = HttpUtility.HtmlDecode (node.Text).ToCharArray ();
			int start = 0;
			int length = ch.Length;
			
			textElementIdx++;
			
			if (flush) {
				FlushBlock ();
				flush = false;
			}
			if (inIgnorableElement != 0) {
				return;
			}
			
			char c;
			bool startWhitespace = false;
			bool endWhitespace = false;
			if (length == 0) {
				return;
			}
			int end = start + length;
			for (int i = start; i < end; i++) {
				if (IsWhiteSpace (ch [i])) {
					ch [i] = ' ';
				}
			}
			while (start < end) {
				c = ch [start];
				if (c == ' ') {
					startWhitespace = true;
					start++;
					length--;
				} else {
					break;
				}
			}
			while (length > 0) {
				c = ch [start + length - 1];
				if (c == ' ') {
					endWhitespace = true;
					length--;
				} else {
					break;
				}
			}
			if (length == 0) {
				if (startWhitespace || endWhitespace) {
					if (!sbLastWasWhitespace) {
						textBuilder.Append (' ');
						tokenBuilder.Append (' ');
					}
					sbLastWasWhitespace = true;
				} else {
					sbLastWasWhitespace = false;
				}
				lastEvent = NBoilerpipeContentHandler.Event.WHITESPACE;
				return;
			}
			if (startWhitespace) {
				if (!sbLastWasWhitespace) {
					textBuilder.Append (' ');
					tokenBuilder.Append (' ');
				}
			}
			if (blockTagLevel == -1) {
				blockTagLevel = tagLevel;
			}
			textBuilder.Append (ch, start, length);
			tokenBuilder.Append (ch, start, length);
			if (endWhitespace) {
				textBuilder.Append (' ');
				tokenBuilder.Append (' ');
			}
			sbLastWasWhitespace = endWhitespace;
			lastEvent = NBoilerpipeContentHandler.Event.CHARACTERS;
			currentContainedTextElements.Add (textElementIdx);
		}
		
		bool IsTag (String text)
		{
			if( (text.StartsWith("</") && text.EndsWith(">")) ||
			    (text.StartsWith("<") && text.EndsWith(">")) )
			{
				return true;
			}
			return false;
		}
		
		bool IsWhiteSpace (char ch)
		{
			if (ch == '\u00A0') return false;
			return char.IsWhiteSpace (ch);
		}
		
		public void FlushBlock ()
		{
			if (inBody == 0) {
				if (inBody == 0 && Sharpen.Runtime.EqualsIgnoreCase ("TITLE", lastStartTag)) 
					SetTitle (tokenBuilder.ToString ().Trim ());
				textBuilder.Length = 0;
				tokenBuilder.Length = 0;
				return;
			}

			int length = tokenBuilder.Length;
			if (length == 0) {
				return;
			} else if (length == 1) {
				if (sbLastWasWhitespace) {
					textBuilder.Length = 0;
					tokenBuilder.Length = 0;
					return;
				}
			}

			string[] tokens = UnicodeTokenizer.Tokenize (tokenBuilder);
			int numWords = 0;
			int numLinkedWords = 0;
			int numWrappedLines = 0;
			int currentLineLength = -1; // don't count the first space
			int maxLineLength = 80;
			int numTokens = 0;
			int numWordsCurrentLine = 0;

			foreach (string token in tokens) {
				if (token == ANCHOR_TEXT_START) {
					inAnchorText = true;
				} else {
					if (token == ANCHOR_TEXT_END) {
						inAnchorText = false;
					} else {
						if (IsWord (token)) {
							numTokens++;
							numWords++;
							numWordsCurrentLine++;
							
							if (inAnchorText) {
								numLinkedWords++;
							}
							int tokenLength = token.Length;
							currentLineLength += tokenLength + 1;
							if (currentLineLength > maxLineLength) {
								numWrappedLines++;
								currentLineLength = tokenLength;
								numWordsCurrentLine = 1;
							}
						} else {
							numTokens++;
						}
					}
				}
			}
			if (numTokens == 0) {
				return;
			}
			int numWordsInWrappedLines;
			if (numWrappedLines == 0) {
				numWordsInWrappedLines = numWords;
				numWrappedLines = 1;
			} else {
				numWordsInWrappedLines = numWords - numWordsCurrentLine;
			}
			TextBlock tb = new TextBlock (textBuilder.ToString ().Trim (), currentContainedTextElements
				, numWords, numLinkedWords, numWordsInWrappedLines, numWrappedLines, offsetBlocks
				);
			currentContainedTextElements = new BitSet ();
			offsetBlocks++;
			textBuilder.Length = 0;
			tokenBuilder.Length = 0;
			tb.SetTagLevel (blockTagLevel);
			AddTextBlock (tb);
			blockTagLevel = -1;
		}

		static bool IsWord (string token)
		{
			return PAT_VALID_WORD_CHARACTER.Matcher (token).Find ();
		}

        public TextDocument ToTextDocument()
        {
            return new TextDocument(title, textBlocks);
        }

        protected void AddTextBlock (TextBlock tb)
		{
			foreach (int l in fontSizeStack) {
				tb.AddLabels ("font-" + l);
				break;
			}
			
			foreach (List<LabelAction> labels in labelStacks) {
				if (labels != null) {
					foreach (LabelAction label in labels) {
						label.AddTo (tb);
					}
				}
			}
			textBlocks.Add (tb);
		}
		
		
		public void AddWhitespaceIfNecessary ()
		{
			if (!sbLastWasWhitespace) {
				tokenBuilder.Append (' ');
				textBuilder.Append (' ');
				sbLastWasWhitespace = true;
			}
		}
		
		public void AddLabelAction (LabelAction la)
		{
			List<LabelAction> labelStack = labelStacks.GetLast ();
			if (labelStack == null) {
				labelStack = new List<LabelAction> ();
				labelStacks.RemoveLast ();
				labelStacks.AddItem (labelStack);
			}
			labelStack.AddItem (la);
		}
		
		public void SetTitle (string s)
		{
			if (s == null || s.Length == 0) {
				return;
			}
			title = s;
		}


    }
}
