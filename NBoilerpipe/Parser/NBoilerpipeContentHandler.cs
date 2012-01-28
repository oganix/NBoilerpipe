using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NBoilerpipe.Parser;
using NBoilerpipe.Document;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe
{
    public class NBoilerpipeContentHandler : IContentHandler
    {
        private String title = null;
        private StringBuilder textBuilder = new StringBuilder();
        private StringBuilder tokenBuilder = new StringBuilder();

        private List<TextBlock> textBlocks = new List<TextBlock>();
        private BitSet currentContainedTextElements = new BitSet();
        private int textElementIndex = 0;
        private int inAnchorElement = 0;
        private int offsetBlocks = 0;
        private bool inAnchorText = false;

        LinkedList<LabelAction> labelStack = new LinkedList<LabelAction>();
        LinkedList<int> fontSizeStack = new LinkedList<int>();

        private static readonly String ANCHOR_TEXT_START = "$\ue00a<";
	    private static readonly String ANCHOR_TEXT_END = ">\ue00a$";
        private List<String> Blacklist = new List<String>
        {
                { "script" },
                { "style" },
                { "option" },
                { "object" },
                { "embed" },
                { "applet" },
                { "link" },
                { "iframe" },
                { "noscript" },
                { "img" },
                { "meta" },
                { "head" },
                { "ul" },
                { "ol" },
                { "li" },
                { "form" },
                { "input" }
        };

        public bool ElementNode (HtmlNode node)
		{
			bool cont = true;
			
			if (Blacklist.Contains (node.Name.ToLowerInvariant ())) {
				cont = false;
			} else if (node.Name.ToLowerInvariant ().Equals ("a")) {
				inAnchorElement++;
				tokenBuilder.Append (ANCHOR_TEXT_START);
				tokenBuilder.Append (' ');
			}
			
			return cont;
		}

        public bool TextNode (HtmlTextNode node)
		{
			bool cont = true;
			
			textElementIndex++;

			String text = null;
			if (!String.IsNullOrEmpty (node.Text)) {
				text = Regex.Replace (node.InnerText.Trim (), "\\s+", " ");
				textBuilder.Append (text);
				tokenBuilder.Append (text);

				if (inAnchorElement != 0) {
					tokenBuilder.Append (ANCHOR_TEXT_END);
					tokenBuilder.Append (' ');
					inAnchorElement--;
				}

				currentContainedTextElements.Add (textElementIndex);
			}

			return cont;
		}

        public void FlushBlock ()
		{
			if(String.IsNullOrEmpty(tokenBuilder.ToString())) return;
			
			char[] seps = { '"', '\'', '@', '!', '-', ':', ';', '?', '$', '/', '(', ')', '.', ',' };
			String[] tokens = tokenBuilder.ToString ().Split (seps);
            
			int numWords = 0;
			int numLinkedWords = 0;
			int numWrappedLines = 0;
			int currentLineLength = 0;
			int maxLineLength = 80;
			int numTokens = 0;
			int numWordsCurrentLine = 0;

			foreach (String token in tokens) {
				if (ANCHOR_TEXT_START.Equals (token)) {
					inAnchorText = true;
				} else if (ANCHOR_TEXT_END.Equals (token)) {
					inAnchorText = false;
				} else if (Regex.IsMatch (token, "\\w+")) {
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

			TextBlock tb = new TextBlock (textBuilder.ToString ().Trim (),
                    currentContainedTextElements, numWords, numLinkedWords,
                    numWordsInWrappedLines, numWrappedLines, offsetBlocks);
			currentContainedTextElements = new BitSet ();

			offsetBlocks++;

			textBuilder.Length = 0;
			tokenBuilder.Length = 0;

			AddTextBlock (tb);
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

		    foreach (LabelAction labels in labelStack) {
			    if (labels != null) {
				    labels.AddLabelsTo(tb);
			    }
		    }
		    textBlocks.Add(tb);
	    }
    }
}
