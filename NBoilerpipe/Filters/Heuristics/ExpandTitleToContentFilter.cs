/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.Heuristics;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe.Filters.Heuristics
{
	/// <summary>
	/// Marks all
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s "content" which are between the headline and the part that
	/// has already been marked content, if they are marked
	/// <see cref="NBoilerpipe.Labels.DefaultLabels.MIGHT_BE_CONTENT">NBoilerpipe.Labels.DefaultLabels.MIGHT_BE_CONTENT
	/// 	</see>
	/// .
	/// This filter is quite specific to the news domain.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class ExpandTitleToContentFilter : BoilerpipeFilter
	{
		public static readonly ExpandTitleToContentFilter INSTANCE = new ExpandTitleToContentFilter
			();

		/// <summary>Returns the singleton instance for ExpandTitleToContentFilter.</summary>
		/// <remarks>Returns the singleton instance for ExpandTitleToContentFilter.</remarks>
		public static ExpandTitleToContentFilter GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			int i = 0;
			int title = -1;
			int contentStart = -1;
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				if (contentStart == -1 && tb.HasLabel(DefaultLabels.TITLE))
				{
					title = i;
					contentStart = -1;
				}
				if (contentStart == -1 && tb.IsContent())
				{
					contentStart = i;
				}
				i++;
			}
			if (contentStart <= title || title == -1)
			{
				return false;
			}
			bool changes = false;
			foreach (TextBlock tb_1 in doc.GetTextBlocks().SubList(title, contentStart))
			{
				if (tb_1.HasLabel(DefaultLabels.MIGHT_BE_CONTENT))
				{
					changes = tb_1.SetIsContent(true) | changes;
				}
			}
			return changes;
		}
	}
}
