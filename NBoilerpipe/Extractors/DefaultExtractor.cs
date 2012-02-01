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
	/// <summary>A quite generic full-text extractor.</summary>
	/// <remarks>A quite generic full-text extractor.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class DefaultExtractor : ExtractorBase
	{
		public static readonly DefaultExtractor INSTANCE = new DefaultExtractor();

		/// <summary>
		/// Returns the singleton instance for
		/// <see cref="DefaultExtractor">DefaultExtractor</see>
		/// .
		/// </summary>
		public static DefaultExtractor GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public override bool Process (TextDocument doc)
		{
			bool ret = SimpleBlockFusionProcessor.INSTANCE.Process (doc) 
				   | BlockProximityFusion.MAX_DISTANCE_1.Process (doc) 
				   | DensityRulesClassifier.INSTANCE.Process (doc);
			return ret;
		}
	}
}
