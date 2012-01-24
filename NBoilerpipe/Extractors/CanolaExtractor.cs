/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using Sharpen;

namespace NBoilerpipe.Extractors
{
	/// <summary>
	/// A full-text extractor trained on <a href="http://krdwrd.org/">krdwrd</a> &lt;a
	/// href
	/// ="https://krdwrd.org/trac/attachment/wiki/Corpora/Canola/CANOLA.pdf"&gt;Canola
	/// </a>.
	/// </summary>
	/// <remarks>
	/// A full-text extractor trained on <a href="http://krdwrd.org/">krdwrd</a> &lt;a
	/// href
	/// ="https://krdwrd.org/trac/attachment/wiki/Corpora/Canola/CANOLA.pdf"&gt;Canola
	/// </a>. Works well with
	/// <see cref="NBoilerpipe.Estimators.SimpleEstimator">NBoilerpipe.Estimators.SimpleEstimator
	/// 	</see>
	/// , too.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class CanolaExtractor : ExtractorBase
	{
		public static readonly CanolaExtractor INSTANCE = new CanolaExtractor();

		/// <summary>
		/// Returns the singleton instance for
		/// <see cref="CanolaExtractor">CanolaExtractor</see>
		/// .
		/// </summary>
		public static CanolaExtractor GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public override bool Process(TextDocument doc)
		{
			return CLASSIFIER.Process(doc);
		}

		private sealed class _BoilerpipeFilter_56 : BoilerpipeFilter
		{
			public _BoilerpipeFilter_56()
			{
			}

			/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
			public bool Process (TextDocument doc)
			{
				IList<TextBlock> textBlocks = doc.GetTextBlocks ();
				bool hasChanges = false;
				ListIterator<TextBlock> it = textBlocks.ListIterator ();
				if (!it.HasNext())
				{
					return false;
				}
				TextBlock prevBlock = TextBlock.EMPTY_START;
				TextBlock currentBlock = it.Next();
				TextBlock nextBlock = it.HasNext() ? it.Next() : TextBlock.EMPTY_START;
				hasChanges = this.Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
				if (nextBlock != TextBlock.EMPTY_START)
				{
					while (it.HasNext())
					{
						prevBlock = currentBlock;
						currentBlock = nextBlock;
						nextBlock = it.Next();
						hasChanges = this.Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
					}
					prevBlock = currentBlock;
					currentBlock = nextBlock;
					nextBlock = TextBlock.EMPTY_START;
					hasChanges = this.Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
				}
				return hasChanges;
			}

			protected internal bool Classify(TextBlock prev, TextBlock curr, TextBlock next)
			{
				bool isContent = (curr.GetLinkDensity() > 0 && next.GetNumWords() > 11) || (curr.
					GetNumWords() > 19 || (next.GetNumWords() > 6 && next.GetLinkDensity() == 0 && prev
					.GetLinkDensity() == 0 && (curr.GetNumWords() > 6 || prev.GetNumWords() > 7 || next
					.GetNumWords() > 19)));
				return curr.SetIsContent(isContent);
			}
		}

		/// <summary>The actual classifier, exposed.</summary>
		/// <remarks>The actual classifier, exposed.</remarks>
		public static readonly BoilerpipeFilter CLASSIFIER = new _BoilerpipeFilter_56();
	}
}
