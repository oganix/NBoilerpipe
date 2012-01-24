/*
 * This code is derived from boilerpipe
 * 
 */

using NBoilerpipe.Conditions;
using NBoilerpipe.Document;
using Sharpen;

namespace NBoilerpipe.Conditions
{
	/// <summary>
	/// Evaluates whether a given
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// meets a certain condition.
	/// Useful in combination with
	/// <see cref="NBoilerpipe.Labels.ConditionalLabelAction">NBoilerpipe.Labels.ConditionalLabelAction
	/// 	</see>
	/// .
	/// </summary>
	/// <author>Christian Kohlschuetter</author>
	public interface TextBlockCondition
	{
		/// <summary>
		/// Returns <code>true</code> iff the given
		/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
		/// tb meets the defined condition.
		/// </summary>
		/// <param name="tb"></param>
		/// <returns><code><true&lt;/code> iff the condition is met.</returns>
		bool MeetsCondition(TextBlock tb);
	}
}
