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
	/// <summary>Fuses adjacent blocks if their distance (in blocks) does not exceed a certain limit.
	/// 	</summary>
	/// <remarks>
	/// Fuses adjacent blocks if their distance (in blocks) does not exceed a certain limit.
	/// This probably makes sense only in cases where an upstream filter already has removed some blocks.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class BlockProximityFusion : BoilerpipeFilter
	{
		private readonly int maxBlocksDistance;

		public static readonly NBoilerpipe.Filters.Heuristics.BlockProximityFusion MAX_DISTANCE_1
			 = new NBoilerpipe.Filters.Heuristics.BlockProximityFusion(1, false, false);

		public static readonly NBoilerpipe.Filters.Heuristics.BlockProximityFusion MAX_DISTANCE_1_SAME_TAGLEVEL
			 = new NBoilerpipe.Filters.Heuristics.BlockProximityFusion(1, false, true);

		public static readonly NBoilerpipe.Filters.Heuristics.BlockProximityFusion MAX_DISTANCE_1_CONTENT_ONLY
			 = new NBoilerpipe.Filters.Heuristics.BlockProximityFusion(1, true, false);

		public static readonly NBoilerpipe.Filters.Heuristics.BlockProximityFusion MAX_DISTANCE_1_CONTENT_ONLY_SAME_TAGLEVEL
			 = new NBoilerpipe.Filters.Heuristics.BlockProximityFusion(1, true, true);

		private readonly bool contentOnly;

		private readonly bool sameTagLevelOnly;

		/// <summary>
		/// Creates a new
		/// <see cref="BlockProximityFusion">BlockProximityFusion</see>
		/// instance.
		/// </summary>
		/// <param name="maxBlocksDistance">The maximum distance in blocks.</param>
		/// <param name="contentOnly"></param>
		public BlockProximityFusion(int maxBlocksDistance, bool contentOnly, bool sameTagLevelOnly
			)
		{
			this.maxBlocksDistance = maxBlocksDistance;
			this.contentOnly = contentOnly;
			this.sameTagLevelOnly = sameTagLevelOnly;
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
			TextBlock prevBlock;
			int offset;
			if (contentOnly)
			{
				prevBlock = null;
				offset = 0;
				foreach (TextBlock tb in textBlocks)
				{
					offset++;
					if (tb.IsContent())
					{
						prevBlock = tb;
						break;
					}
				}
				if (prevBlock == null)
				{
					return false;
				}
			}
			else
			{
				prevBlock = textBlocks[0];
				offset = 1;
			}
			for (ListIterator<TextBlock> it = textBlocks.ListIterator<TextBlock>(offset); it.HasNext(); )
			{
				TextBlock block = it.Next();
				if (!block.IsContent())
				{
					prevBlock = block;
					continue;
				}
				int diffBlocks = block.GetOffsetBlocksStart() - prevBlock.GetOffsetBlocksEnd() - 
					1;
				if (diffBlocks <= maxBlocksDistance)
				{
					bool ok = true;
					if (contentOnly)
					{
						if (!prevBlock.IsContent() || !block.IsContent())
						{
							ok = false;
						}
					}
					if (ok && sameTagLevelOnly && prevBlock.GetTagLevel() != block.GetTagLevel())
					{
						ok = false;
					}
					if (ok)
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
				else
				{
					prevBlock = block;
				}
			}
			return changes;
		}
	}
}
