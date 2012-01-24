/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Labels
{
	/// <summary>
	/// Helps adding labels to
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	/// <seealso cref="ConditionalLabelAction">ConditionalLabelAction</seealso>
	public class LabelAction
	{
		protected internal readonly string[] labels;

		public LabelAction(params string[] labels)
		{
			this.labels = labels;
		}

		public virtual void AddTo(TextBlock tb)
		{
			AddLabelsTo(tb);
		}

		protected internal void AddLabelsTo(TextBlock tb)
		{
			tb.AddLabels(labels);
		}

		public override string ToString()
		{
			return base.ToString() + "{" + Arrays.AsList(labels) + "}";
		}
	}
}
