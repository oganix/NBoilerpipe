/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Estimators
{
	/// <summary>
	/// Estimates the "goodness" of a
	/// <see cref="NBoilerpipe.BoilerpipeExtractor">NBoilerpipe.BoilerpipeExtractor</see>
	/// on a given document.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class SimpleEstimator
	{
		/// <summary>
		/// Returns the singleton instance of
		/// <see cref="SimpleEstimator">SimpleEstimator</see>
		/// </summary>
		public static readonly NBoilerpipe.Estimators.SimpleEstimator INSTANCE = new NBoilerpipe.Estimators.SimpleEstimator
			();

		public SimpleEstimator()
		{
		}

		/// <summary>
		/// Given the statistics of the document before and after applying the
		/// <see cref="NBoilerpipe.BoilerpipeExtractor">NBoilerpipe.BoilerpipeExtractor</see>
		/// ,
		/// can we regard the extraction quality (too) low?
		/// Works well with
		/// <see cref="NBoilerpipe.Extractors.DefaultExtractor">NBoilerpipe.Extractors.DefaultExtractor
		/// 	</see>
		/// ,
		/// <see cref="NBoilerpipe.Extractors.ArticleExtractor">NBoilerpipe.Extractors.ArticleExtractor
		/// 	</see>
		/// and others.
		/// </summary>
		/// <param name="dsBefore"></param>
		/// <param name="dsAfter"></param>
		/// <returns>true if low quality is to be expected.</returns>
		public bool IsLowQuality(TextDocumentStatistics dsBefore, TextDocumentStatistics 
			dsAfter)
		{
			if (dsBefore.GetNumWords() < 90 || dsAfter.GetNumWords() < 70)
			{
				return true;
			}
			if (dsAfter.AvgNumWords() < 25)
			{
				return true;
			}
			return false;
		}
	}
}
