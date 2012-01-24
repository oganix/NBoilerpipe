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
	/// Finds blocks which are potentially indicating the end of an article text and
	/// marks them with
	/// <see cref="NBoilerpipe.Labels.DefaultLabels.INDICATES_END_OF_TEXT">NBoilerpipe.Labels.DefaultLabels.INDICATES_END_OF_TEXT
	/// 	</see>
	/// . This can be used
	/// in conjunction with a downstream
	/// <see cref="IgnoreBlocksAfterContentFilter">IgnoreBlocksAfterContentFilter</see>
	/// .
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="IgnoreBlocksAfterContentFilter">IgnoreBlocksAfterContentFilter</seealso>
	public class TerminatingBlocksFinder : BoilerpipeFilter
	{
		public static readonly TerminatingBlocksFinder INSTANCE = new TerminatingBlocksFinder
			();

		/// <summary>Returns the singleton instance for TerminatingBlocksFinder.</summary>
		/// <remarks>Returns the singleton instance for TerminatingBlocksFinder.</remarks>
		public static TerminatingBlocksFinder GetInstance()
		{
			return INSTANCE;
		}

		// public static long timeSpent = 0;
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public virtual bool Process(TextDocument doc)
		{
			bool changes = false;
			// long t = System.currentTimeMillis();
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				int numWords = tb.GetNumWords();
				if (numWords < 15)
				{
					string text = tb.GetText().Trim();
					int len = text.Length;
					if (len >= 8)
					{
						string textLC = text.ToLower();
						if (textLC.StartsWith("comments") || StartsWithNumber(textLC, len, " comments", " users responded in"
							) || textLC.StartsWith("¬© reuters") || textLC.StartsWith("please rate this") ||
							 textLC.StartsWith("post a comment") || textLC.Contains("what you think...") || 
							textLC.Contains("add your comment") || textLC.Contains("add comment") || textLC.
							Contains("reader views") || textLC.Contains("have your say") || textLC.Contains(
							"reader comments") || textLC.Contains("r√§tta artikeln") || textLC.Equals("thanks for your comments - this feedback is now closed"
							))
						{
							tb.AddLabel(DefaultLabels.INDICATES_END_OF_TEXT);
							changes = true;
						}
					}
				}
			}
			// timeSpent += System.currentTimeMillis() - t;
			return changes;
		}

		/// <summary>
		/// Checks whether the given text t starts with a sequence of digits,
		/// followed by one of the given strings.
		/// </summary>
		/// <remarks>
		/// Checks whether the given text t starts with a sequence of digits,
		/// followed by one of the given strings.
		/// </remarks>
		/// <param name="t">The text to examine</param>
		/// <param name="len">The length of the text to examine</param>
		/// <param name="str">Any strings that may follow the digits.</param>
		/// <returns>true if at least one combination matches</returns>
		private static bool StartsWithNumber(string t, int len, params string[] str)
		{
			int j = 0;
			while (j < len && IsDigit(t[j]))
			{
				j++;
			}
			if (j != 0)
			{
				foreach (string s in str)
				{
					if (t.StartsWith(s, j))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}
	}
}
