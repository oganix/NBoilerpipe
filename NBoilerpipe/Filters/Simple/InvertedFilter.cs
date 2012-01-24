/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>
	/// Reverts the "isContent" flag for all
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class InvertedFilter : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Simple.InvertedFilter INSTANCE = new NBoilerpipe.Filters.Simple.InvertedFilter
			();

		public InvertedFilter()
		{
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			IList<TextBlock> tbs = doc.GetTextBlocks();
			if (tbs.IsEmpty())
			{
				return false;
			}
			foreach (TextBlock tb in tbs)
			{
				tb.SetIsContent(!tb.IsContent());
			}
			return true;
		}
	}
}
