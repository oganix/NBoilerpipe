/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Util;
using Sharpen;
using System.Text.RegularExpressions;

namespace NBoilerpipe.Util
{
	/// <summary>
	/// Tokenizes text according to Unicode word boundaries and strips off non-word
	/// characters.
	/// </summary>
	/// <remarks>
	/// Tokenizes text according to Unicode word boundaries and strips off non-word
	/// characters.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public class UnicodeTokenizer
	{
		private static readonly Sharpen.Pattern PAT_WORD_BOUNDARY = Sharpen.Pattern.Compile
			("\\b");

		private static readonly Sharpen.Pattern PAT_NOT_WORD_BOUNDARY = Sharpen.Pattern.Compile
			("[\u2063]*([\\\"'\\.,\\!\\@\\-\\:\\;\\$\\?\\(\\)/])[\u2063]*");


        private static readonly Regex RE_U00A0 = new Regex("\u00A0", RegexOptions.Compiled);
        private static readonly Regex RE_U2063 = new Regex("[ \u2063]+", RegexOptions.Compiled);
        private static readonly Regex RE_SPACE_SPLITTER = new Regex("[ \u2063]+", RegexOptions.Compiled);

		/// <summary>Tokenizes the text and returns an array of tokens.</summary>
		/// <remarks>Tokenizes the text and returns an array of tokens.</remarks>
		/// <param name="text">The text</param>
		/// <returns>The tokens</returns>
		public static string[] Tokenize(CharSequence text)
		{
            string replaced = text.ToString().ReplaceAll(RE_U00A0, "'\u00A0'");
            string match1 = PAT_WORD_BOUNDARY.Matcher(replaced).ReplaceAll("\u2063");
            string match2 = PAT_NOT_WORD_BOUNDARY.Matcher(match1).ReplaceAll("$1");

            string replaced2 = match2.ReplaceAll(RE_U2063, " ");
            string trimmed = replaced2.Trim();
            //string[] parts = trimmed.Split("[ ]+");
            string[] parts = RE_SPACE_SPLITTER.Split(trimmed);

            return parts;
		}
	}
}
