/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using System.Text;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Document
{
	/// <summary>
	/// A text document, consisting of one or more
	/// <see cref="TextBlock">TextBlock</see>
	/// s.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public class TextDocument
	{
		internal readonly IList<TextBlock> textBlocks;

		internal string title;

		/// <summary>
		/// Creates a new
		/// <see cref="TextDocument">TextDocument</see>
		/// with given
		/// <see cref="TextBlock">TextBlock</see>
		/// s, and no
		/// title.
		/// </summary>
		/// <param name="textBlocks">The text blocks of this document.</param>
		public TextDocument(IList<TextBlock> textBlocks) : this(null, textBlocks)
		{
		}

		/// <summary>
		/// Creates a new
		/// <see cref="TextDocument">TextDocument</see>
		/// with given
		/// <see cref="TextBlock">TextBlock</see>
		/// s and
		/// given title.
		/// </summary>
		/// <param name="title">The "main" title for this text document.</param>
		/// <param name="textBlocks">The text blocks of this document.</param>
		public TextDocument(string title, IList<TextBlock> textBlocks)
		{
			this.title = title;
			this.textBlocks = textBlocks;
		}

		/// <summary>
		/// Returns the
		/// <see cref="TextBlock">TextBlock</see>
		/// s of this document.
		/// </summary>
		/// <returns>
		/// A list of
		/// <see cref="TextBlock">TextBlock</see>
		/// s, in sequential order of appearance.
		/// </returns>
		public virtual IList<TextBlock> GetTextBlocks()
		{
			return textBlocks;
		}

		/// <summary>
		/// Returns the "main" title for this document, or <code>null</code> if no
		/// such title has ben set.
		/// </summary>
		/// <remarks>
		/// Returns the "main" title for this document, or <code>null</code> if no
		/// such title has ben set.
		/// </remarks>
		/// <returns>The "main" title.</returns>
		public virtual string GetTitle()
		{
			return title;
		}

		/// <summary>Updates the "main" title for this document.</summary>
		/// <remarks>Updates the "main" title for this document.</remarks>
		/// <param name="title"></param>
		public virtual void SetTitle(string title)
		{
			this.title = title;
		}

		/// <summary>
		/// Returns the
		/// <see cref="TextDocument">TextDocument</see>
		/// 's content.
		/// </summary>
		/// <returns>The content text.</returns>
		public virtual string GetContent()
		{
			return GetText(true, false);
		}

		/// <summary>
		/// Returns the
		/// <see cref="TextDocument">TextDocument</see>
		/// 's content, non-content or both
		/// </summary>
		/// <param name="includeContent">Whether to include TextBlocks marked as "content".</param>
		/// <param name="includeNonContent">Whether to include TextBlocks marked as "non-content".
		/// 	</param>
		/// <returns>The text.</returns>
		public virtual string GetText (bool includeContent, bool includeNonContent)
		{
			StringBuilder sb = new StringBuilder ();
			foreach (TextBlock block in GetTextBlocks()) {
				if (block.IsContent ()) {
					if (!includeContent)
						continue;
				} else {
					if (!includeNonContent)
						continue;
				}
				sb.Append (block.GetText ());
				sb.Append ('\n');
			}
			return sb.ToString ();
		}

		/// <summary>
		/// Returns detailed debugging information about the contained
		/// <see cref="TextBlock">TextBlock</see>
		/// s.
		/// </summary>
		/// <returns>Debug information.</returns>
		public virtual string DebugString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (TextBlock tb in GetTextBlocks())
			{
				sb.Append(tb.ToString());
				sb.Append('\n');
			}
			return sb.ToString();
		}
	}
}
