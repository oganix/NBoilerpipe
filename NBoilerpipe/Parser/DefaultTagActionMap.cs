/*
 * This code is derived from boilerpipe
 * 
 */

using Sharpen;

namespace NBoilerpipe.Parser
{
	/// <summary>
	/// Default
	/// <see cref="TagAction">TagAction</see>
	/// s. Seem to work well.
	/// </summary>
	/// <seealso cref="TagActionMap">TagActionMap</seealso>
	[System.Serializable]
	public class DefaultTagActionMap : TagActionMap
	{
		private const long serialVersionUID = 1L;

		public static readonly TagActionMap INSTANCE = new NBoilerpipe.Parser.DefaultTagActionMap();

		public DefaultTagActionMap()
		{
			SetTagAction("STYLE", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("SCRIPT", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("OPTION", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("OBJECT", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("EMBED", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("APPLET", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("LINK", CommonTagActions.TA_IGNORABLE_ELEMENT);
			SetTagAction("A", CommonTagActions.TA_ANCHOR_TEXT);
			SetTagAction("BODY", CommonTagActions.TA_BODY);
			SetTagAction("STRIKE", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("U", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("B", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("I", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("EM", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("STRONG", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("SPAN", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			// New in 1.1 (especially to improve extraction quality from Wikipedia etc.)
			SetTagAction("SUP", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			// New in 1.2
			SetTagAction("CODE", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("TT", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("SUB", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("VAR", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			SetTagAction("ABBR", CommonTagActions.TA_INLINE_WHITESPACE);
			SetTagAction("ACRONYM", CommonTagActions.TA_INLINE_WHITESPACE);
			SetTagAction("FONT", CommonTagActions.TA_INLINE_NO_WHITESPACE);
			// could also use TA_FONT 
			// added in 1.1.1
			SetTagAction("NOSCRIPT", CommonTagActions.TA_IGNORABLE_ELEMENT);
		}
	}
}
