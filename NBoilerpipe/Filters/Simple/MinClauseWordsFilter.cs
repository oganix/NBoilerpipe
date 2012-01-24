/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>
	/// Keeps only blocks that have at least one segment fragment ("clause") with at
	/// least <em>k</em> words (default: 5).
	/// </summary>
	/// <remarks>
	/// Keeps only blocks that have at least one segment fragment ("clause") with at
	/// least <em>k</em> words (default: 5).
	/// NOTE: You might consider using the
	/// <see cref="SplitParagraphBlocksFilter">SplitParagraphBlocksFilter</see>
	/// upstream.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="SplitParagraphBlocksFilter">SplitParagraphBlocksFilter</seealso>
	public sealed class MinClauseWordsFilter : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Simple.MinClauseWordsFilter INSTANCE = 
			new NBoilerpipe.Filters.Simple.MinClauseWordsFilter(5, false);

		private int minWords;

		private readonly bool acceptClausesWithoutDelimiter;

		public MinClauseWordsFilter(int minWords) : this(minWords, false)
		{
		}

		public MinClauseWordsFilter(int minWords, bool acceptClausesWithoutDelimiter)
		{
			this.minWords = minWords;
			this.acceptClausesWithoutDelimiter = acceptClausesWithoutDelimiter;
		}

		private readonly Sharpen.Pattern PAT_CLAUSE_DELIMITER = Sharpen.Pattern.Compile("[\\p{L}\\d][\\,\\.\\:\\;\\!\\?]+([ \\n\\r]+|$)"
			);

		private readonly Sharpen.Pattern PAT_WHITESPACE = Sharpen.Pattern.Compile("[ \\n\\r]+"
			);

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			bool changes = false;
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				if (!tb.IsContent())
				{
					continue;
				}
				string text = tb.GetText();
				Matcher m = PAT_CLAUSE_DELIMITER.Matcher(text);
				bool found = m.Find();
				int start = 0;
				int end;
				bool hasClause = false;
				while (found)
				{
					end = m.Start() + 1;
					hasClause = IsClause(text.SubSequence(start, end));
					start = m.End();
					if (hasClause)
					{
						break;
					}
					found = m.Find();
				}
				end = text.Length;
				// since clauses should *always end* with a delimiter, we normally
				// don't consider text without one
				if (acceptClausesWithoutDelimiter)
				{
					hasClause |= IsClause(text.SubSequence(start, end));
				}
				if (!hasClause)
				{
					tb.SetIsContent(false);
					changes = true;
				}
			}
			// System.err.println("IS NOT CONTENT: " + text);
			return changes;
		}

		private bool IsClause(CharSequence text)
		{
			Matcher m = PAT_WHITESPACE.Matcher(text.ToString());
			int n = 1;
			while (m.Find())
			{
				n++;
				if (n >= minWords)
				{
					return true;
				}
			}
			return n >= minWords;
		}
	}
}
