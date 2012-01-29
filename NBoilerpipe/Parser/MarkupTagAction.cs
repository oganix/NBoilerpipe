/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe.Labels;
using Sharpen;
using HtmlAgilityPack;

namespace NBoilerpipe.Parser
{
	/// <summary>
	/// Assigns labels for element CSS classes and ids to the corresponding
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// . CSS classes are prefixed by
	/// <code>
	/// <see cref="NBoilerpipe.Labels.DefaultLabels.MARKUP_PREFIX">NBoilerpipe.Labels.DefaultLabels.MARKUP_PREFIX
	/// 	</see>
	/// .</code>, and IDs are prefixed by
	/// <code>
	/// <see cref="NBoilerpipe.Labels.DefaultLabels.MARKUP_PREFIX">NBoilerpipe.Labels.DefaultLabels.MARKUP_PREFIX
	/// 	</see>
	/// #</code>
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class MarkupTagAction : TagAction
	{
		private readonly bool isBlockLevel;

		private List<IList<string>> labelStack = new List<IList<string>>();

		public MarkupTagAction(bool isBlockLevel)
		{
			this.isBlockLevel = isBlockLevel;
		}

		private static readonly Sharpen.Pattern PAT_NUM = Sharpen.Pattern.Compile("[0-9]+"
			);

		/// <exception cref="Sharpen.SAXException"></exception>
		public bool Start (NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
		{
			IList<string> labels = new AList<string> (5);
			labels.AddItem (DefaultLabels.MARKUP_PREFIX + localName);
			string classVal = atts ["class"].Value;
			if (classVal != null && classVal.Length > 0) {
				classVal = PAT_NUM.Matcher (classVal).ReplaceAll ("#");
				classVal = classVal.Trim ();
				string[] vals = classVal.Split ("[ ]+");
				labels.AddItem (DefaultLabels.MARKUP_PREFIX + "." + classVal.Replace (' ', '.'));
				if (vals.Length > 1) {
					foreach (string s in vals) {
						labels.AddItem (DefaultLabels.MARKUP_PREFIX + "." + s);
					}
				}
			}
			var att = atts["id"];
			var id =  ( atts !=null) ? att.Name : "";
			if (id != null && id.Length > 0) {
				id = PAT_NUM.Matcher (id).ReplaceAll ("#");
				labels.AddItem (DefaultLabels.MARKUP_PREFIX + "#" + id);
			}
			ICollection<string> ancestors = GetAncestorLabels ();
			IList<string> labelsWithAncestors = new AList<string> ((ancestors.Count + 1) * labels
				.Count);
			foreach (string l in labels) {
				foreach (string an in ancestors) {
					labelsWithAncestors.AddItem (an);
					labelsWithAncestors.AddItem (an + " " + l);
				}
				labelsWithAncestors.AddItem (l);
			}
			instance.AddLabelAction (new LabelAction (Sharpen.Collections.ToArray (labelsWithAncestors
				, new string[labelsWithAncestors.Count])));
			labelStack.AddItem (labels);
			return isBlockLevel;
		}

		/// <exception cref="Sharpen.SAXException"></exception>
		public bool End(NBoilerpipeContentHandler instance, string localName)
		{
			labelStack.RemoveLast();
			return isBlockLevel;
		}

		public bool ChangesTagLevel()
		{
			return isBlockLevel;
		}

		private ICollection<string> GetAncestorLabels()
		{
			ICollection<string> set = new HashSet<string>();
			foreach (IList<string> labels in labelStack)
			{
				if (labels == null)
				{
					continue;
				}
				Sharpen.Collections.AddAll(set, labels);
			}
			return set;
		}
	}
}
