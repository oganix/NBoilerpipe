/*
 * This code is derived from boilerpipe
 * 
 */

using System.IO;
using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe
{
	/// <summary>Describes a complete filter pipeline.</summary>
	/// <remarks>Describes a complete filter pipeline.</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public interface BoilerpipeExtractor : BoilerpipeFilter
	{
		/// <summary>Extracts text from the HTML code given as a String.</summary>
		/// <remarks>Extracts text from the HTML code given as a String.</remarks>
		/// <param name="html">The HTML code as a String.</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		string GetText(string html);

		/// <summary>
		/// Extracts text from the HTML code available from the given
		/// <see cref="Org.Xml.Sax.InputSource">Org.Xml.Sax.InputSource</see>
		/// .
		/// </summary>
		/// <param name="is">The InputSource containing the HTML</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		//string GetText(InputSource @is);

		/// <summary>
		/// Extracts text from the HTML code available from the given
		/// <see cref="System.IO.StreamReader">System.IO.StreamReader</see>
		/// .
		/// </summary>
		/// <param name="r">The Reader containing the HTML</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		//string GetText(StreamReader r);

		/// <summary>
		/// Extracts text from the given
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// object.
		/// </summary>
		/// <param name="doc">
		/// The
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// .
		/// </param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		string GetText(TextDocument doc);
	}
}
