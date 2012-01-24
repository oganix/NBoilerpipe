/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.Heuristics;
using Sharpen;

namespace NBoilerpipe.Filters.Heuristics
{
	/// <summary>Merges two subsequent blocks if their text densities are equal.</summary>
	/// <remarks>Merges two subsequent blocks if their text densities are equal.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class SimpleBlockFusionProcessor : BoilerpipeFilter
	{
		public static readonly SimpleBlockFusionProcessor INSTANCE = new SimpleBlockFusionProcessor
			();

		/// <summary>Returns the singleton instance for BlockFusionProcessor.</summary>
		/// <remarks>Returns the singleton instance for BlockFusionProcessor.</remarks>
		public static SimpleBlockFusionProcessor GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public virtual bool Process(TextDocument doc)
		{
			IList<TextBlock> textBlocks = doc.GetTextBlocks();
			bool changes = false;
			if (textBlocks.Count < 2)
			{
				return false;
			}
			TextBlock b1 = textBlocks[0];
			for (ListIterator<TextBlock> it = textBlocks.ListIterator(1); it.HasNext(); )
			{
				TextBlock b2 = it.Next();
				bool similar = (b1.GetTextDensity() == b2.GetTextDensity());
				if (similar)
				{
					b1.MergeNext(b2);
					it.Remove();
					changes = true;
				}
				else
				{
					b1 = b2;
				}
			}
			return changes;
		}
	}
}
