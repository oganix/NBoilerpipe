/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Conditions;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	public class SurroundingToContentFilter : BoilerpipeFilter
	{
		private sealed class _TextBlockCondition_13 : TextBlockCondition
		{
			public _TextBlockCondition_13()
			{
			}

			public bool MeetsCondition(TextBlock tb)
			{
				return tb.GetLinkDensity() == 0 && tb.GetNumWords() > 6;
			}
		}

		public static readonly NBoilerpipe.Filters.Simple.SurroundingToContentFilter INSTANCE_TEXT
			 = new NBoilerpipe.Filters.Simple.SurroundingToContentFilter(new _TextBlockCondition_13
			());

		private readonly TextBlockCondition cond;

		public SurroundingToContentFilter(TextBlockCondition cond)
		{
			this.cond = cond;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public virtual bool Process(TextDocument doc)
		{
			IList<TextBlock> tbs = doc.GetTextBlocks();
			if (tbs.Count < 3)
			{
				return false;
			}
			TextBlock a = tbs[0];
			TextBlock b = tbs[1];
			TextBlock c;
			bool hasChanges = false;
			for (ListIterator<TextBlock> it = tbs.ListIterator(2); it.HasNext(); )
			{
				c = it.Next();
				if (!b.IsContent() && a.IsContent() && c.IsContent() && cond.MeetsCondition(b))
				{
					b.SetIsContent(true);
					hasChanges = true;
				}
				a = c;
				if (!it.HasNext())
				{
					break;
				}
				b = it.Next();
			}
			return hasChanges;
		}
	}
}
