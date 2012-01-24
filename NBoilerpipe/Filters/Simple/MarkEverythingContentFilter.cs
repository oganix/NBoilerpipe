/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>Marks all blocks as content.</summary>
	/// <remarks>Marks all blocks as content.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class MarkEverythingContentFilter : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Simple.MarkEverythingContentFilter INSTANCE
			 = new NBoilerpipe.Filters.Simple.MarkEverythingContentFilter();

		public MarkEverythingContentFilter()
		{
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			bool changes = false;
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				if (!tb.IsContent())
				{
					tb.SetIsContent(true);
					changes = true;
				}
			}
			return changes;
		}
	}
}
