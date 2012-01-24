/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Filters.Simple;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>
	/// Removes
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s which have explicitly been marked as "not content".
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class BoilerplateBlockFilter : BoilerpipeFilter
	{
		public static readonly BoilerplateBlockFilter INSTANCE = new BoilerplateBlockFilter
			();

		/// <summary>Returns the singleton instance for BoilerplateBlockFilter.</summary>
		/// <remarks>Returns the singleton instance for BoilerplateBlockFilter.</remarks>
		public static BoilerplateBlockFilter GetInstance()
		{
			return INSTANCE;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			IList<TextBlock> textBlocks = doc.GetTextBlocks();
			bool hasChanges = false;
			for (Iterator<TextBlock> it = textBlocks.Iterator(); it.HasNext(); )
			{
				TextBlock tb = it.Next();
				if (!tb.IsContent())
				{
					it.Remove();
					hasChanges = true;
				}
			}
			return hasChanges;
		}
	}
}
