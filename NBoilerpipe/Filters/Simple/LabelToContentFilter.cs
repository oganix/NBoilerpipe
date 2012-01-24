/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>Marks all blocks that contain a given label as "content".</summary>
	/// <remarks>Marks all blocks that contain a given label as "content".</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class LabelToContentFilter : BoilerpipeFilter
	{
		private string[] labels;

		public LabelToContentFilter(params string[] label)
		{
			this.labels = label;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process (TextDocument doc)
		{
			bool changes = false;
			foreach (TextBlock tb in doc.GetTextBlocks()) {
				if (!tb.IsContent ()) {
					foreach (string label in labels) {
						if (tb.HasLabel (label)) {
							tb.SetIsContent (true);
							changes = true;
							goto BLOCK_LOOP_continue;
						}
					}
				}
			BLOCK_LOOP_continue:{}
			}
			return changes;
		}
	}
}
