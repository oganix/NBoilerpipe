/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe.Filters.Simple
{
	/// <summary>Marks all blocks that contain a given label as "boilerplate".</summary>
	/// <remarks>Marks all blocks that contain a given label as "boilerplate".</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class LabelToBoilerplateFilter : BoilerpipeFilter
	{
		public static readonly NBoilerpipe.Filters.Simple.LabelToBoilerplateFilter INSTANCE_STRICTLY_NOT_CONTENT
			 = new NBoilerpipe.Filters.Simple.LabelToBoilerplateFilter(DefaultLabels.STRICTLY_NOT_CONTENT
			);

		private string[] labels;

		public LabelToBoilerplateFilter(params string[] label)
		{
			this.labels = label;
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process (TextDocument doc)
		{
			bool changes = false;
			foreach (TextBlock tb in doc.GetTextBlocks()) {
				if (tb.IsContent ()) {
					foreach (string label in labels) {
						if (tb.HasLabel (label)) {
							tb.SetIsContent (false);
							changes = true;
							goto BLOCK_LOOP_continue;
						}
					}
					BLOCK_LOOP_continue: {}
				}
			}
			return changes;
		}
	}
}
