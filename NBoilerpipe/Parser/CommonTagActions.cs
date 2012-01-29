/*
 * This code is derived from boilerpipe
 * 
 */

using System;
using NBoilerpipe.Labels;
using Sharpen;
using HtmlAgilityPack;

namespace NBoilerpipe.Parser
{
	/// <summary>Defines an action that is to be performed whenever a particular tag occurs during HTML parsing.
	/// 	</summary>
	/// <remarks>Defines an action that is to be performed whenever a particular tag occurs during HTML parsing.
	/// 	</remarks>
	/// <author>Christian Kohlsch√ºtter</author>
	public abstract class CommonTagActions
	{
		public CommonTagActions()
		{
		}

		public sealed class Chained : TagAction
		{
			private readonly TagAction t1;

			private readonly TagAction t2;

			public Chained(TagAction t1, TagAction t2)
			{
				this.t1 = t1;
				this.t2 = t2;
			}

			/// <exception cref="Sharpen.SAXException"></exception>
			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				return t1.Start(instance, localName, atts) | t2.Start(instance, localName,atts);
			}

			/// <exception cref="Sharpen.SAXException"></exception>
			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				return t1.End(instance, localName) | t2.End(instance, localName);
			}

			public bool ChangesTagLevel()
			{
				return t1.ChangesTagLevel() || t2.ChangesTagLevel();
			}
		}

		private sealed class _TagAction_70 : TagAction
		{
			public _TagAction_70()
			{
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName,HtmlAttributeCollection atts)
			{
				instance.inIgnorableElement++;
				return true;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName	)
			{
				instance.inIgnorableElement--;
				return true;
			}

			public bool ChangesTagLevel()
			{
				return true;
			}
		}

		/// <summary>Marks this tag as "ignorable", i.e.</summary>
		/// <remarks>Marks this tag as "ignorable", i.e. all its inner content is silently skipped.
		/// 	</remarks>
		public static readonly TagAction TA_IGNORABLE_ELEMENT = new _TagAction_70();

		private sealed class _TagAction_97 : TagAction
		{
			public _TagAction_97()
			{
			}

			/// <exception cref="Sharpen.SAXException"></exception>
			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				if (instance.inAnchor++ > 0)
				{
					// as nested A elements are not allowed per specification, we
					// are probably reaching this branch due to a bug in the XML
					// parser
					System.Console.Error.WriteLine("Warning: SAX input contains nested A elements -- You have probably hit a bug in your HTML parser (e.g., NekoHTML bug #2909310). Please clean the HTML externally and feed it to boilerpipe again. Trying to recover somehow..."
						);
					this.End(instance, localName);
				}
				if (instance.inIgnorableElement == 0)
				{
					instance.AddWhitespaceIfNecessary();
					instance.tokenBuilder.Append(NBoilerpipeContentHandler.ANCHOR_TEXT_START);
					instance.tokenBuilder.Append(' ');
					instance.sbLastWasWhitespace = true;
				}
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				if (--instance.inAnchor == 0)
				{
					if (instance.inIgnorableElement == 0)
					{
						instance.AddWhitespaceIfNecessary();
						instance.tokenBuilder.Append(NBoilerpipeContentHandler.ANCHOR_TEXT_END);
						instance.tokenBuilder.Append(' ');
						instance.sbLastWasWhitespace = true;
					}
				}
				return false;
			}

			public bool ChangesTagLevel()
			{
				return true;
			}
		}

		/// <summary>Marks this tag as "anchor" (this should usually only be set for the <code>&lt;A&gt;</code> tag).
		/// 	</summary>
		/// <remarks>
		/// Marks this tag as "anchor" (this should usually only be set for the <code>&lt;A&gt;</code> tag).
		/// Anchor tags may not be nested.
		/// There is a bug in certain versions of NekoHTML which still allows nested tags.
		/// If boilerpipe encounters such nestings, a SAXException is thrown.
		/// </remarks>
		public static readonly TagAction TA_ANCHOR_TEXT = new _TagAction_97();

		private sealed class _TagAction_142 : TagAction
		{
			public _TagAction_142()
			{
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				instance.FlushBlock();
				instance.inBody++;
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				instance.FlushBlock();
				instance.inBody--;
				return false;
			}

			public bool ChangesTagLevel()
			{
				return true;
			}
		}

		/// <summary>Marks this tag the body element (this should usually only be set for the <code>&lt;BODY&gt;</code> tag).
		/// 	</summary>
		/// <remarks>Marks this tag the body element (this should usually only be set for the <code>&lt;BODY&gt;</code> tag).
		/// 	</remarks>
		public static readonly TagAction TA_BODY = new _TagAction_142();

		private sealed class _TagAction_166 : TagAction
		{
			public _TagAction_166()
			{
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				instance.AddWhitespaceIfNecessary();
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				instance.AddWhitespaceIfNecessary();
				return false;
			}

			public bool ChangesTagLevel()
			{
				return false;
			}
		}

		/// <summary>Marks this tag a simple "inline" element, which generates whitespace, but no new block.
		/// 	</summary>
		/// <remarks>Marks this tag a simple "inline" element, which generates whitespace, but no new block.
		/// 	</remarks>
		public static readonly TagAction TA_INLINE_WHITESPACE = new _TagAction_166();

		[System.ObsoleteAttribute(@"Use TA_INLINE_WHITESPACE instead")]
		public static readonly TagAction TA_INLINE = TA_INLINE_WHITESPACE;

		private sealed class _TagAction_195 : TagAction
		{
			public _TagAction_195()
			{
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				return false;
			}

			public bool ChangesTagLevel()
			{
				return false;
			}
		}

		/// <summary>Marks this tag a simple "inline" element, which neither generates whitespace, nor a new block.
		/// 	</summary>
		/// <remarks>Marks this tag a simple "inline" element, which neither generates whitespace, nor a new block.
		/// 	</remarks>
		public static readonly TagAction TA_INLINE_NO_WHITESPACE = new _TagAction_195();
		private static readonly Sharpen.Pattern PAT_FONT_SIZE = Sharpen.Pattern.Compile("([\\+\\-]?)([0-9])");

		private sealed class _TagAction_218 : TagAction
		{
			public _TagAction_218()
			{
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				return true;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				return true;
			}

			public bool ChangesTagLevel()
			{
				return true;
			}
		}

		/// <summary>Explicitly marks this tag a simple "block-level" element, which always generates whitespace
		/// 	</summary>
		public static readonly TagAction TA_BLOCK_LEVEL = new _TagAction_218();

		private sealed class _TagAction_240 : TagAction
		{
			public _TagAction_240()
			{
			}

			public bool Start (NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				string sizeAttr = atts ["size"].Value;
				if (sizeAttr != null)
				{
					Matcher m = CommonTagActions.PAT_FONT_SIZE.Matcher(sizeAttr);
					if (m.Matches())
					{
						string rel = m.Group(1);
						int val = System.Convert.ToInt32(m.Group(2));
						int size;
						if (rel.Length == 0)
						{
							// absolute
							size = val;
						}
						else
						{
							// relative
							int? prevSize;
							if (instance.fontSizeStack.IsEmpty())
							{
								prevSize = 3;
							}
							else
							{
								prevSize = 3;
								foreach (int? s in instance.fontSizeStack)
								{
									if (s != null)
									{
										prevSize = s;
										break;
									}
								}
							}
							if (rel[0] == '+')
							{
								size = (int)prevSize + val;
							}
							else
							{
								size = (int)prevSize - val;
							}
						}
						instance.fontSizeStack.Add(0, size);
					}
					else
					{
						instance.fontSizeStack.Add(0, null);
					}
				}
				else
				{
					instance.fontSizeStack.Add(0, null);
				}
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				instance.fontSizeStack.RemoveFirst();
				return false;
			}

			public bool ChangesTagLevel()
			{
				return false;
			}
		}

		/// <summary>
		/// Special TagAction for the <code>&lt;FONT&gt;</code> tag, which keeps track of the
		/// absolute and relative font size.
		/// </summary>
		/// <remarks>
		/// Special TagAction for the <code>&lt;FONT&gt;</code> tag, which keeps track of the
		/// absolute and relative font size.
		/// </remarks>
		public static readonly TagAction TA_FONT = new _TagAction_240();

		/// <summary>
		/// <see cref="CommonTagActions">CommonTagActions</see>
		/// for inline elements, which triggers some
		/// <see cref="NBoilerpipe.Labels.LabelAction">NBoilerpipe.Labels.LabelAction</see>
		/// on the generated
		/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
		/// .
		/// </summary>
		public sealed class InlineTagLabelAction : TagAction
		{
			private readonly LabelAction action;

			public InlineTagLabelAction(LabelAction action)
			{
				this.action = action;
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				instance.AddWhitespaceIfNecessary();
				instance.AddLabelAction(action);
				return false;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				instance.AddWhitespaceIfNecessary();
				return false;
			}

			public bool ChangesTagLevel()
			{
				return false;
			}
		}

		/// <summary>
		/// <see cref="CommonTagActions">CommonTagActions</see>
		/// for block-level elements, which triggers some
		/// <see cref="NBoilerpipe.Labels.LabelAction">NBoilerpipe.Labels.LabelAction</see>
		/// on the generated
		/// <see cref="NBoilerpipe.Document.TextBlock">NBoilerpipe.Document.TextBlock</see>
		/// .
		/// </summary>
		public sealed class BlockTagLabelAction : TagAction
		{
			private readonly LabelAction action;

			public BlockTagLabelAction(LabelAction action)
			{
				this.action = action;
			}

			public bool Start(NBoilerpipeContentHandler instance, string localName, HtmlAttributeCollection atts)
			{
				instance.AddLabelAction(action);
				return true;
			}

			public bool End(NBoilerpipeContentHandler instance, string localName)
			{
				return true;
			}

			public bool ChangesTagLevel()
			{
				return true;
			}
		}
	}
}
