/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using NBoilerpipe.Filters.Simple;
using Sharpen;

namespace NBoilerpipe.Extractors
{
	/// <summary>Marks everything as content.</summary>
	/// <remarks>Marks everything as content.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class KeepEverythingExtractor : ExtractorBase
	{
		public static readonly NBoilerpipe.Extractors.KeepEverythingExtractor INSTANCE = 
			new NBoilerpipe.Extractors.KeepEverythingExtractor();

		public KeepEverythingExtractor()
		{
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public override bool Process(TextDocument doc)
		{
			return MarkEverythingContentFilter.INSTANCE.Process(doc);
		}
	}
}
