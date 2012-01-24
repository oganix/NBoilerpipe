/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using NBoilerpipe.Filters.English;
using NBoilerpipe.Filters.Heuristics;
using Sharpen;

namespace NBoilerpipe.Extractors
{
	/// <summary>A full-text extractor which extracts the largest text component of a page.
	/// 	</summary>
	/// <remarks>
	/// A full-text extractor which extracts the largest text component of a page.
	/// For news articles, it may perform better than the
	/// <see cref="DefaultExtractor">DefaultExtractor</see>
	/// ,
	/// but usually worse than
	/// <see cref="ArticleExtractor">ArticleExtractor</see>
	/// .
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class LargestContentExtractor : ExtractorBase
	{
		public static readonly NBoilerpipe.Extractors.LargestContentExtractor INSTANCE = 
			new NBoilerpipe.Extractors.LargestContentExtractor();

		public LargestContentExtractor()
		{
		}

		/// <summary>
		/// Returns the singleton instance for
		/// <see cref="LargestContentExtractor">LargestContentExtractor</see>
		/// .
		/// </summary>
		public static NBoilerpipe.Extractors.LargestContentExtractor GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public override bool Process(TextDocument doc)
		{
			return NumWordsRulesClassifier.INSTANCE.Process(doc) | BlockProximityFusion.MAX_DISTANCE_1
				.Process(doc) | KeepLargestBlockFilter.INSTANCE.Process(doc);
		}
	}
}
