/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe.Filters.Heuristics
{
	/// <summary>Fuses adjacent blocks if their labels are equal.</summary>
	/// <remarks>Fuses adjacent blocks if their labels are equal.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class LabelFusion : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Heuristics.LabelFusion INSTANCE = new 
			NBoilerpipe.Filters.Heuristics.LabelFusion(string.Empty);

		private readonly string labelPrefix;

		/// <summary>
		/// Creates a new
		/// <see cref="LabelFusion">LabelFusion</see>
		/// instance.
		/// </summary>
		/// <param name="maxBlocksDistance">The maximum distance in blocks.</param>
		/// <param name="contentOnly"></param>
		public LabelFusion(string labelPrefix)
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
			TextBlock prevBlock = textBlocks[0];
			int offset = 1;
			for (ListIterator<TextBlock> it = textBlocks.ListIterator(offset); it.HasNext(); )
			{
				TextBlock block = it.Next();
				if (EqualLabels(prevBlock.GetLabels(), block.GetLabels()))
				{
					prevBlock.MergeNext(block);
					it.Remove();
					changes = true;
				}
				else
				{
					prevBlock = block;
				}
			}
			return changes;
		}

		private bool EqualLabels(ICollection<string> labels, ICollection<string> labels2)
		{
			if (labels == null || labels2 == null)
			{
				return false;
			}
			return MarkupLabelsOnly(labels).Equals(MarkupLabelsOnly(labels2));
		}

		private ICollection<string> MarkupLabelsOnly(ICollection<string> set1)
		{
			ICollection<string> set = new HashSet<string>(set1);
			for (Iterator<string> it = set.Iterator(); it.HasNext(); )
			{
				if (!it.Next().StartsWith(DefaultLabels.MARKUP_PREFIX))
				{
					it.Remove();
				}
			}
			return set;
		}
	}
}
