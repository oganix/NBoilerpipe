/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using NBoilerpipe;
using NBoilerpipe.Document;
using NBoilerpipe.Labels;
using Sharpen;

namespace NBoilerpipe.Filters.Heuristics
{
	/// <summary>
	/// Marks
	/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
	/// s which contain parts of the HTML
	/// <code>&lt;TITLE&gt;</code> tag, using some heuristics which are quite
	/// specific to the news domain.
	/// </summary>
	/// <author>Christian Kohlsch√ºtter</author>
	public sealed class DocumentTitleMatchClassifier : BoilerpipeFilter
	{
		private readonly ICollection<string> potentialTitles;

		public DocumentTitleMatchClassifier(string title)
		{
			if (title == null)
			{
				this.potentialTitles = null;
			}
			else
			{
				title = title.Trim();
				if (title.Length == 0)
				{
					this.potentialTitles = null;
				}
				else
				{
					this.potentialTitles = new HashSet<string>();
					potentialTitles.AddItem(title);
					string p;
					p = GetLongestPart(title, "[ ]*[\\|¬ª|:][ ]*");
					if (p != null)
					{
						potentialTitles.AddItem(p);
					}
					p = GetLongestPart(title, "[ ]*[\\|¬ª|:\\(\\)][ ]*");
					if (p != null)
					{
						potentialTitles.AddItem(p);
					}
					p = GetLongestPart(title, "[ ]*[\\|¬ª|:\\(\\)\\-][ ]*");
					if (p != null)
					{
						potentialTitles.AddItem(p);
					}
					p = GetLongestPart(title, "[ ]*[\\|¬ª|,|:\\(\\)\\-][ ]*");
					if (p != null)
					{
						potentialTitles.AddItem(p);
					}
				}
			}
		}

		public ICollection<string> GetPotentialTitles()
		{
			return potentialTitles;
		}

		private string GetLongestPart(string title, string pattern)
		{
			string[] parts = title.Split(pattern);
			if (parts.Length == 1)
			{
				return null;
			}
			int longestNumWords = 0;
			string longestPart = string.Empty;
			for (int i = 0; i < parts.Length; i++)
			{
				string p = parts[i];
				if (p.Contains(".com"))
				{
					continue;
				}
				int numWords = p.Split("[\b]+").Length;
				if (numWords > longestNumWords || p.Length > longestPart.Length)
				{
					longestNumWords = numWords;
					longestPart = p;
				}
			}
			if (longestPart.Length == 0)
			{
				return null;
			}
			else
			{
				return longestPart.Trim();
			}
		}

		/// <exception cref="NBoilerpipe.BoilerpipeProcessingException"></exception>
		public bool Process(TextDocument doc)
		{
			if (potentialTitles == null)
			{
				return false;
			}
			bool changes = false;
			foreach (TextBlock tb in doc.GetTextBlocks())
			{
				string text = tb.GetText().Trim();
				foreach (string candidate in potentialTitles)
				{
					if (candidate.Equals(text))
					{
						tb.AddLabel(DefaultLabels.TITLE);
						changes = true;
					}
				}
			}
			return changes;
		}
	}
}
