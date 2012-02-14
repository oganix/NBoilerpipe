/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Util;
using Sharpen;

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

		/// <summary>Tokenizes the text and returns an array of tokens.</summary>
		/// <remarks>Tokenizes the text and returns an array of tokens.</remarks>
		/// <param name="text">The text</param>
		/// <returns>The tokens</returns>
		public static string[] Tokenize(CharSequence text)
		{
			return PAT_NOT_WORD_BOUNDARY.Matcher(PAT_WORD_BOUNDARY.Matcher(text.ToString().ReplaceAll ("\u00A0","'\u00A0'")).ReplaceAll("\u2063"
				)).ReplaceAll("$1").ReplaceAll("[ \u2063]+", " ").Trim().Split("[ ]+");
		}
	}
}
