/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe
{
	/// <summary>
	/// Something that can be represented as a
	/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
	/// .
	/// </summary>
	public interface BoilerpipeDocumentSource
	{
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		TextDocument ToTextDocument();
	}
}
