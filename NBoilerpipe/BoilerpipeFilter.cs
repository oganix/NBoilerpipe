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
	/// A generic
	/// <see cref="BoilerpipeFilter">BoilerpipeFilter</see>
	/// . Takes a
	/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
	/// and
	/// processes it somehow.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public interface BoilerpipeFilter
	{
		/// <summary>Processes the given document <code>doc</code>.</summary>
		/// <remarks>Processes the given document <code>doc</code>.</remarks>
		/// <param name="doc">
		/// The
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// that is to be processed.
		/// </param>
		/// <returns>
		/// <code>true</code> if changes have been made to the
		/// <see cref="NBoilerpipe.Document.TextDocument">NBoilerpipe.Document.TextDocument</see>
		/// .
		/// </returns>
		/// <exception cref="BoilerpipeProcessingException">BoilerpipeProcessingException</exception>
		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		bool Process(TextDocument doc);
	}
}
