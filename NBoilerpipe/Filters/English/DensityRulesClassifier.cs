/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.English;
using Sharpen;

namespace NBoilerpipe.Filters.English
{
	/// <summary>
	/// Classifies
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s as content/not-content through rules that have
	/// been determined using the C4.8 machine learning algorithm, as described in the
	/// paper "Boilerplate Detection using Shallow Text Features", particularly using
	/// text densities and link densities.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public class DensityRulesClassifier : BoilerpipeFilter
	{
		public static readonly DensityRulesClassifier INSTANCE = new DensityRulesClassifier
			();

		/// <summary>Returns the singleton instance for RulebasedBoilerpipeClassifier.</summary>
		/// <remarks>Returns the singleton instance for RulebasedBoilerpipeClassifier.</remarks>
		public static DensityRulesClassifier GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public virtual bool Process(TextDocument doc)
		{
			IList<TextBlock> textBlocks = doc.GetTextBlocks();
			bool hasChanges = false;
			ListIterator<TextBlock> it = textBlocks.ListIterator();
			if (!it.HasNext())
			{
				return false;
			}
			TextBlock prevBlock = TextBlock.EMPTY_START;
			TextBlock currentBlock = it.Next();
			TextBlock nextBlock = it.HasNext() ? it.Next() : TextBlock.EMPTY_START;
			hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
			if (nextBlock != TextBlock.EMPTY_START)
			{
				while (it.HasNext())
				{
					prevBlock = currentBlock;
					currentBlock = nextBlock;
					nextBlock = it.Next();
					hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
				}
				prevBlock = currentBlock;
				currentBlock = nextBlock;
				nextBlock = TextBlock.EMPTY_START;
				hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
			}
			return hasChanges;
		}

		protected internal virtual bool Classify(TextBlock prev, TextBlock curr, TextBlock
			 next)
		{
			bool isContent;
			if (curr.GetLinkDensity() <= 0.333333)
			{
				if (prev.GetLinkDensity() <= 0.555556)
				{
					if (curr.GetTextDensity() <= 9)
					{
						if (next.GetTextDensity() <= 10)
						{
							if (prev.GetTextDensity() <= 4)
							{
								isContent = false;
							}
							else
							{
								isContent = true;
							}
						}
						else
						{
							isContent = true;
						}
					}
					else
					{
						if (next.GetTextDensity() == 0)
						{
							isContent = false;
						}
						else
						{
							isContent = true;
						}
					}
				}
				else
				{
					if (next.GetTextDensity() <= 11)
					{
						isContent = false;
					}
					else
					{
						isContent = true;
					}
				}
			}
			else
			{
				isContent = false;
			}
			return curr.SetIsContent(isContent);
		}
	}
}
