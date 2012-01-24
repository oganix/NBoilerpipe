/*
 * This code is derived from boilerpipe
 * 
 */

using System;
using System.IO;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Extractors;
using NBoilerpipe.Parser;
using Sharpen;

namespace NBoilerpipe.Extractors
{
	/// <summary>The base class of Extractors.</summary>
	/// <remarks>
	/// The base class of Extractors. Also provides some helper methods to quickly
	/// retrieve the text that remained after processing.
	/// </remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public abstract class ExtractorBase : BoilerpipeExtractor
	{
		/// <summary>Extracts text from the HTML code given as a String.</summary>
		/// <remarks>Extracts text from the HTML code given as a String.</remarks>
		/// <param name="html">The HTML code as a String.</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException">NBoilerpipe.BoilerpipeProcessingException
		/// 	</exception>
		public virtual string GetText (string html)
		{
			try {

				NBoilerpipeHtmlParser parser = new NBoilerpipeHtmlParser (new NBoilerpipeContentHandler ());
				parser.Parse (html);
				return GetText (parser.ToTextDocument ());
			} catch (Exception e) {
				throw new BoilerpipeProcessingException (e.ToString());
			}
		}

		/// <summary>
		/// Extracts text from the HTML code available from the given
		/// <see cref="Org.Xml.Sax.InputSource">Org.Xml.Sax.InputSource</see>
		/// .
		/// </summary>
		/// <param name="is">The InputSource containing the HTML</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException">NBoilerpipe.BoilerpipeProcessingException
		/// 	</exception>
		/*public virtual string GetText(InputSource @is)
		{
			try
			{
				return GetText(new BoilerpipeSAXInput(@is).GetTextDocument());
			}
			catch (SAXException e)
			{
				throw new BoilerpipeProcessingException(e);
			}
		}*/

		/// <summary>
		/// Extracts text from the HTML code available from the given
		/// <see cref="System.Uri">System.Uri</see>
		/// .
		/// NOTE: This method is mainly to be used for show case purposes. If you are
		/// going to crawl the Web, consider using
		/// <see cref="GetText(Org.Xml.Sax.InputSource)">GetText(Org.Xml.Sax.InputSource)</see>
		/// instead.
		/// </summary>
		/// <param name="url">The URL pointing to the HTML code.</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException">NBoilerpipe.BoilerpipeProcessingException
		/// 	</exception>
		/*public virtual string GetText(Uri url)
		{
			try
			{
				return GetText(HTMLFetcher.Fetch(url).ToInputSource());
			}
			catch (IOException e)
			{
				throw new BoilerpipeProcessingException(e);
			}
		}
		*/

		/// <summary>
		/// Extracts text from the HTML code available from the given
		/// <see cref="System.IO.StreamReader">System.IO.StreamReader</see>
		/// .
		/// </summary>
		/// <param name="r">The Reader containing the HTML</param>
		/// <returns>The extracted text.</returns>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException">NBoilerpipe.BoilerpipeProcessingException
		/// 	</exception>
		/*public virtual string GetText(StreamReader r)
		{
			return GetText(new InputSource(r));
		}
		*/

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
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException">NBoilerpipe.BoilerpipeProcessingException
		/// 	</exception>
		public virtual string GetText(TextDocument doc)
		{
			Process(doc);
			return doc.GetContent();
		}

		public abstract bool Process(TextDocument arg1);
	}
}
