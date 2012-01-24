/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.English;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe.Filters.English
{
	/// <summary>
	/// Marks all blocks as "non-content" that occur after blocks that have been
	/// marked
	/// <see cref="NBoilerpipe.Labels.DefaultLabels.INDICATES_END_OF_TEXT">NBoilerpipe.Labels.DefaultLabels.INDICATES_END_OF_TEXT
	/// 	</see>
	/// , and after any content block.
	/// This filter can be used in conjunction with an upstream
	/// <see cref="TerminatingBlocksFinder">TerminatingBlocksFinder</see>
	/// .
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="TerminatingBlocksFinder">TerminatingBlocksFinder</seealso>
	public sealed class IgnoreBlocksAfterContentFromEndFilter : HeuristicFilterBase, 
		BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFromEndFilter
			 INSTANCE = new NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFromEndFilter
			();

		public IgnoreBlocksAfterContentFromEndFilter()
		{
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			bool changes = false;
			int words = 0;
			IList<TextBlock> blocks = doc.GetTextBlocks();
			if (!blocks.IsEmpty())
			{
				ListIterator<TextBlock> it = blocks.ListIterator<TextBlock>(blocks.Count);
				TextBlock tb;
				while (it.HasPrevious())
				{
					tb = it.Previous();
					if (tb.HasLabel(DefaultLabels.INDICATES_END_OF_TEXT))
					{
						tb.AddLabel(DefaultLabels.STRICTLY_NOT_CONTENT);
						tb.RemoveLabel(DefaultLabels.MIGHT_BE_CONTENT);
						tb.SetIsContent(false);
						changes = true;
					}
					else
					{
						if (tb.IsContent())
						{
							words += tb.GetNumWords();
							if (words > 200)
							{
								break;
							}
						}
					}
				}
			}
			return changes;
		}
	}
}
