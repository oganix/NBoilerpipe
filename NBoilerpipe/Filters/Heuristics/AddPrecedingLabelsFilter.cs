/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Heuristics
{
	/// <summary>Adds the labels of the preceding block to the current block, optionally adding a prefix.
	/// 	</summary>
	/// <remarks>Adds the labels of the preceding block to the current block, optionally adding a prefix.
	/// 	</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class AddPrecedingLabelsFilter : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Heuristics.AddPrecedingLabelsFilter INSTANCE
			 = new NBoilerpipe.Filters.Heuristics.AddPrecedingLabelsFilter(string.Empty);

		public static readonly NBoilerpipe.Filters.Heuristics.AddPrecedingLabelsFilter INSTANCE_PRE
			 = new NBoilerpipe.Filters.Heuristics.AddPrecedingLabelsFilter("^");

		private readonly string labelPrefix;

		/// <summary>
		/// Creates a new
		/// <see cref="AddPrecedingLabelsFilter">AddPrecedingLabelsFilter</see>
		/// instance.
		/// </summary>
		/// <param name="maxBlocksDistance">The maximum distance in blocks.</param>
		/// <param name="contentOnly"></param>
		public AddPrecedingLabelsFilter(string labelPrefix)
		{
			this.labelPrefix = labelPrefix;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			IList<TextBlock> textBlocks = doc.GetTextBlocks();
			if (textBlocks.Count < 2)
			{
				return false;
			}
			bool changes = false;
			int remaining = textBlocks.Count;
			TextBlock blockBelow = null;
			TextBlock block;
			for (ListIterator<TextBlock> it = textBlocks.ListIterator<TextBlock>(textBlocks.Count); it.HasPrevious
				(); )
			{
				if (--remaining <= 0)
				{
					break;
				}
				if (blockBelow == null)
				{
					blockBelow = it.Previous();
					continue;
				}
				block = it.Previous();
				ICollection<string> labels = block.GetLabels();
				if (labels != null && !labels.IsEmpty())
				{
					foreach (string l in labels)
					{
						blockBelow.AddLabel(labelPrefix + l);
					}
					changes = true;
				}
				blockBelow = block;
			}
			return changes;
		}
	}
}
