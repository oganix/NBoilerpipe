/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.Simple;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>Splits TextBlocks at paragraph boundaries.</summary>
	/// <remarks>
	/// Splits TextBlocks at paragraph boundaries.
	/// NOTE: This is not fully supported (i.e., it will break highlighting support
	/// via #getContainedTextElements()), but this one probably is necessary for some other
	/// filters.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="MinClauseWordsFilter">MinClauseWordsFilter</seealso>
	public sealed class SplitParagraphBlocksFilter : BoilerpipeFilter
	{
		public static readonly SplitParagraphBlocksFilter INSTANCE = new SplitParagraphBlocksFilter
			();

		/// <summary>Returns the singleton instance for TerminatingBlocksFinder.</summary>
		/// <remarks>Returns the singleton instance for TerminatingBlocksFinder.</remarks>
		public static SplitParagraphBlocksFilter GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			bool changes = false;
			IList<TextBlock> blocks = doc.GetTextBlocks();
			IList<TextBlock> blocksNew = new AList<TextBlock>();
			foreach (TextBlock tb in blocks)
			{
				string text = tb.GetText();
				string[] paragraphs = text.Split("[\n\r]+");
				if (paragraphs.Length < 2)
				{
					blocksNew.AddItem(tb);
					continue;
				}
				bool isContent = tb.IsContent();
				ICollection<string> labels = tb.GetLabels();
				foreach (string p in paragraphs)
				{
					TextBlock tbP = new TextBlock(p);
					tbP.SetIsContent(isContent);
					tbP.AddLabels(labels);
					blocksNew.AddItem(tbP);
					changes = true;
				}
			}
			if (changes)
			{
				blocks.Clear();
				Sharpen.Collections.AddAll(blocks, blocksNew);
			}
			return changes;
		}
	}
}
