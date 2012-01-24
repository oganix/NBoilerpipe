/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Document
{
	/// <summary>Provides shallow statistics on a given TextDocument</summary>
	/// <author>Christian Kohlschuetter</author>
	public sealed class TextDocumentStatistics
	{
		private int numWords = 0;

		private int numBlocks = 0;

		/// <summary>
		/// Computes statistics on a given
		/// <see cref="TextDocument">TextDocument</see>
		/// .
		/// </summary>
		/// <param name="doc">
		/// The
		/// <see cref="TextDocument">TextDocument</see>
		/// .
		/// </param>
		/// <param name="contentOnly">if true then o</param>
		public TextDocumentStatistics(TextDocument doc, bool contentOnly)
		{
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				if (contentOnly && !tb.IsContent())
				{
					continue;
				}
				numWords += tb.GetNumWords();
				numBlocks++;
			}
		}

		/// <summary>
		/// Returns the average number of words at block-level (= overall number of words divided by
		/// the number of blocks).
		/// </summary>
		/// <remarks>
		/// Returns the average number of words at block-level (= overall number of words divided by
		/// the number of blocks).
		/// </remarks>
		/// <returns>Average</returns>
		public float AvgNumWords()
		{
			return numWords / (float)numBlocks;
		}

		/// <summary>Returns the overall number of words in all blocks.</summary>
		/// <remarks>Returns the overall number of words in all blocks.</remarks>
		/// <returns>Sum</returns>
		public int GetNumWords()
		{
			return numWords;
		}
	}
}
