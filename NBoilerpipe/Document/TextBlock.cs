/*
 * This code is derived from boilerpipe
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Sharpen;

namespace NBoilerpipe.Document
{
	/// <summary>Describes a block of text.</summary>
	/// <remarks>
	/// Describes a block of text.
	/// A block can be an "atomic" text element (i.e., a sequence of text that is not
	/// interrupted by any HTML markup) or a compound of such atomic elements.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class TextBlock : ICloneable
	{
		internal bool isContent = false;
		CharSequence text;
		internal ICollection<string> labels = null;
		internal int offsetBlocksStart;
		internal int offsetBlocksEnd;
		internal int numWords;
		internal int numWordsInAnchorText;
		internal int numWordsInWrappedLines;
		internal int numWrappedLines;
		internal float textDensity;
		internal float linkDensity;
		internal BitSet containedTextElements;
		int numFullTextWords = 0;
		int tagLevel;
		static readonly BitSet EMPTY_BITSET = new BitSet();

		public static readonly NBoilerpipe.Document.TextBlock EMPTY_START = new NBoilerpipe.Document.TextBlock
			(string.Empty, EMPTY_BITSET, 0, 0, 0, 0, -1);

		public static readonly NBoilerpipe.Document.TextBlock EMPTY_END = new NBoilerpipe.Document.TextBlock
			(string.Empty, EMPTY_BITSET, 0, 0, 0, 0, int.MaxValue);

		public TextBlock(string text) : this(text, null, 0, 0, 0, 0, 0)
		{
		}

		public TextBlock(string text, BitSet containedTextElements, int numWords, int numWordsInAnchorText
			, int numWordsInWrappedLines, int numWrappedLines, int offsetBlocks)
		{
			this.text = text;
			this.containedTextElements = containedTextElements;
			this.numWords = numWords;
			this.numWordsInAnchorText = numWordsInAnchorText;
			this.numWordsInWrappedLines = numWordsInWrappedLines;
			this.numWrappedLines = numWrappedLines;
			this.offsetBlocksStart = offsetBlocks;
			this.offsetBlocksEnd = offsetBlocks;
			InitDensities();
		}

		public virtual bool IsContent()
		{
			return isContent;
		}

		public virtual bool SetIsContent(bool isContent)
		{
			if (isContent != this.isContent)
			{
				this.isContent = isContent;
				return true;
			}
			else
			{
				return false;
			}
		}

		public virtual string GetText()
		{
			return text.ToString();
		}

		public virtual int GetNumWords()
		{
			return numWords;
		}

		public virtual int GetNumWordsInAnchorText()
		{
			return numWordsInAnchorText;
		}

		public virtual float GetTextDensity()
		{
			return textDensity;
		}

		public virtual float GetLinkDensity()
		{
			return linkDensity;
		}

		public virtual void MergeNext (NBoilerpipe.Document.TextBlock other)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (text);
			sb.Append ('\n');
			sb.Append (other.text);
			this.text = sb.ToString ();
			numWords += other.numWords;
			numWordsInAnchorText += other.numWordsInAnchorText;
			numWordsInWrappedLines += other.numWordsInWrappedLines;
			numWrappedLines += other.numWrappedLines;
			offsetBlocksStart = Math.Min (offsetBlocksStart, other.offsetBlocksStart);
			offsetBlocksEnd = Math.Max (offsetBlocksEnd, other.offsetBlocksEnd);
			InitDensities ();
			this.isContent |= other.isContent;
			if (containedTextElements == null) {
				containedTextElements = (BitSet)other.containedTextElements.Clone ();
			} else {
				containedTextElements.Or (other.containedTextElements);
			}
			numFullTextWords += other.numFullTextWords;
			if (other.labels != null) {
				if (labels == null) {
					labels = new HashSet<string> (other.labels);
				} else {
					Sharpen.Collections.AddAll (labels, other.labels);
				}
			}
			tagLevel = Math.Min (tagLevel, other.tagLevel);
		}

		private void InitDensities()
		{
			if (numWordsInWrappedLines == 0)
			{
				numWordsInWrappedLines = numWords;
				numWrappedLines = 1;
			}
			textDensity = numWordsInWrappedLines / (float)numWrappedLines;
			linkDensity = numWords == 0 ? 0 : numWordsInAnchorText / (float)numWords;
		}

		public virtual int GetOffsetBlocksStart()
		{
			return offsetBlocksStart;
		}

		public virtual int GetOffsetBlocksEnd()
		{
			return offsetBlocksEnd;
		}

		public override string ToString()
		{
			return "[" + offsetBlocksStart + "-" + offsetBlocksEnd + ";tl=" + tagLevel + "; nw="
				 + numWords + ";nwl=" + numWrappedLines + ";ld=" + linkDensity + "]\t" + (isContent
				 ? "CONTENT" : "boilerplate") + "," + labels + "\n" + GetText();
		}

		/// <summary>
		/// Adds an arbitrary String label to this
		/// <see cref="TextBlock">TextBlock</see>
		/// .
		/// </summary>
		/// <param name="label">The label</param>
		/// <seealso cref="NBoilerpipe.Labels.DefaultLabels">NBoilerpipe.Labels.DefaultLabels
		/// 	</seealso>
		public virtual void AddLabel(string label)
		{
			if (labels == null)
			{
				labels = new HashSet<string>();
			}
			labels.AddItem(label);
		}

		/// <summary>Checks whether this TextBlock has the given label.</summary>
		/// <remarks>Checks whether this TextBlock has the given label.</remarks>
		/// <param name="label">The label</param>
		/// <returns><code>true</code> if this block is marked by the given label.</returns>
		public virtual bool HasLabel(string label)
		{
			return labels != null && labels.Contains(label);
		}

		public virtual bool RemoveLabel(string label)
		{
			return labels != null && labels.Remove(label);
		}

		/// <summary>
		/// Returns the labels associated to this TextBlock, or <code>null</code> if no such labels
		/// exist.
		/// </summary>
		/// <remarks>
		/// Returns the labels associated to this TextBlock, or <code>null</code> if no such labels
		/// exist.
		/// NOTE: The returned instance is the one used directly in TextBlock. You have full access
		/// to the data structure. However it is recommended to use the label-specific methods in
		/// <see cref="TextBlock">TextBlock</see>
		/// whenever possible.
		/// </remarks>
		/// <returns>Returns the set of labels, or <code>null</code> if no labels was added yet.
		/// 	</returns>
		public virtual ICollection<string> GetLabels()
		{
			return labels;
		}

		/// <summary>
		/// Adds a set of labels to this
		/// <see cref="TextBlock">TextBlock</see>
		/// .
		/// <code>null</code>-references are silently ignored.
		/// </summary>
		/// <param name="l">The labels to be added.</param>
		public virtual void AddLabels(ICollection<string> l)
		{
			if (l == null)
			{
				return;
			}
			if (this.labels == null)
			{
				this.labels = new HashSet<string>(l);
			}
			else
			{
				Sharpen.Collections.AddAll(this.labels, l);
			}
		}

		/// <summary>
		/// Adds a set of labels to this
		/// <see cref="TextBlock">TextBlock</see>
		/// .
		/// <code>null</code>-references are silently ignored.
		/// </summary>
		/// <param name="l">The labels to be added.</param>
		public virtual void AddLabels(params string[] l)
		{
			if (l == null)
			{
				return;
			}
			if (this.labels == null)
			{
				this.labels = new HashSet<string>();
			}
			foreach (string label in l)
			{
				this.labels.AddItem(label);
			}
		}

		/// <summary>Returns the containedTextElements BitSet, or <code>null</code>.</summary>
		/// <remarks>Returns the containedTextElements BitSet, or <code>null</code>.</remarks>
		/// <returns></returns>
		public virtual BitSet GetContainedTextElements()
		{
			return containedTextElements;
		}

		public Object Clone ()
		{
			TextBlock clone = new TextBlock (text.ToString());

			if (labels != null && !labels.IsEmpty ()) {
				clone.labels = new HashSet<string> (labels);
			}
			if (containedTextElements != null) {
				clone.containedTextElements = (BitSet)containedTextElements.Clone ();
			}
			return clone;
		}

		public virtual int GetTagLevel()
		{
			return tagLevel;
		}

		public virtual void SetTagLevel(int tagLevel)
		{
			this.tagLevel = tagLevel;
		}
	}
}
