/*
 * This code is derived from boilerpipe
 * 
 */

using Sharpen;
using HtmlAgilityPack;

namespace NBoilerpipe.Parser
{
	/// <summary>
	/// Defines an action that is to be performed whenever a particular tag occurs
	/// during HTML parsing.
	/// </summary>
	/// <remarks>
	/// Defines an action that is to be performed whenever a particular tag occurs
	/// during HTML parsing.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public interface TagAction
	{
		/// <exception cref="Sharpen.SAXException"></exception>
		bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts);

		/// <exception cref="Sharpen.SAXException"></exception>
		bool End(NBoilerpipeContentHandler instance, string localName);

		bool ChangesTagLevel();
	}
}
