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
	/// A source that returns
	/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
	/// s.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public interface BoilerpipeInput
	{
		/// <summary>
		/// Returns (somehow) a
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// .
		/// </summary>
		/// <returns>
		/// A
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// .
		/// </returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		TextDocument GetTextDocument();
	}
}
