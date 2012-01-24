/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using NBoilerpipe.Filters.English;
using Sharpen;

namespace NBoilerpipe.Extractors
{
	/// <summary>
	/// A quite generic full-text extractor solely based upon the number of words per
	/// block (the current, the previous and the next block).
	/// </summary>
	/// <remarks>
	/// A quite generic full-text extractor solely based upon the number of words per
	/// block (the current, the previous and the next block).
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class NumWordsRulesExtractor : ExtractorBase
	{
		public static readonly NumWordsRulesExtractor INSTANCE = new NumWordsRulesExtractor
			();

		/// <summary>
		/// Returns the singleton instance for
		/// <see cref="NumWordsRulesExtractor">NumWordsRulesExtractor</see>
		/// .
		/// </summary>
		public static NumWordsRulesExtractor GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public override bool Process(TextDocument doc)
		{
			return NumWordsRulesClassifier.INSTANCE.Process(doc);
		}
	}
}
