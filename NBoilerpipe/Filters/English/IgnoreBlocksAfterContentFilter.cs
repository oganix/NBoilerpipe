/*
 * This code is derived from boilerpipe
 * 
 */

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
	/// . These marks are ignored
	/// unless a minimum number of words in content blocks occur before this mark (default: 60).
	/// This can be used in conjunction with an upstream
	/// <see cref="TerminatingBlocksFinder">TerminatingBlocksFinder</see>
	/// .
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="TerminatingBlocksFinder">TerminatingBlocksFinder</seealso>
	public sealed class IgnoreBlocksAfterContentFilter : HeuristicFilterBase, BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFilter
			 DEFAULT_INSTANCE = new NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFilter
			(60);

		public static readonly NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFilter
			 INSTANCE_200 = new NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFilter(200
			);

		private readonly int minNumWords;

		/// <summary>Returns the singleton instance for DeleteBlocksAfterContentFilter.</summary>
		/// <remarks>Returns the singleton instance for DeleteBlocksAfterContentFilter.</remarks>
		public static NBoilerpipe.Filters.English.IgnoreBlocksAfterContentFilter GetDefaultInstance
			()
		{
			return DEFAULT_INSTANCE;
		}

		public IgnoreBlocksAfterContentFilter(int minNumWords)
		{
			this.minNumWords = minNumWords;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			bool changes = false;
			int numWords = 0;
			bool foundEndOfText = false;
			for (Iterator<TextBlock> it = doc.GetTextBlocks().Iterator(); it.HasNext(); )
			{
				TextBlock block = it.Next();
				bool endOfText = block.HasLabel(DefaultLabels.INDICATES_END_OF_TEXT);
				if (block.IsContent())
				{
					numWords += GetNumFullTextWords(block);
				}
				if (endOfText && numWords >= minNumWords)
				{
					foundEndOfText = true;
				}
				if (foundEndOfText)
				{
					changes = true;
					block.SetIsContent(false);
				}
			}
			return changes;
		}
	}
}
