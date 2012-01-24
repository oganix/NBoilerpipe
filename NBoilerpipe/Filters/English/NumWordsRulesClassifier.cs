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
	/// been determined using the C4.8 machine learning algorithm, as described in
	/// the paper "Boilerplate Detection using Shallow Text Features" (WSDM 2010),
	/// particularly using number of words per block and link density per block.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public class NumWordsRulesClassifier : BoilerpipeFilter
	{
		public static readonly NumWordsRulesClassifier INSTANCE = new NumWordsRulesClassifier
			();

		/// <summary>Returns the singleton instance for RulebasedBoilerpipeClassifier.</summary>
		/// <remarks>Returns the singleton instance for RulebasedBoilerpipeClassifier.</remarks>
		public static NumWordsRulesClassifier GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public virtual bool Process(TextDocument doc)
		{
			IList<TextBlock> textBlocks = doc.GetTextBlocks();
			bool hasChanges = false;
			ListIterator<TextBlock> it = textBlocks.ListIterator(0);
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
					if (curr.GetNumWords() <= 16)
					{
						if (next.GetNumWords() <= 15)
						{
							if (prev.GetNumWords() <= 4)
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
						isContent = true;
					}
				}
				else
				{
					if (curr.GetNumWords() <= 40)
					{
						if (next.GetNumWords() <= 17)
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
			}
			else
			{
				isContent = false;
			}
			return curr.SetIsContent(isContent);
		}
	}
}
