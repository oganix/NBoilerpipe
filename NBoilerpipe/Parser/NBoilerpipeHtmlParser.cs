using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using HtmlAgilityPack;
using NBoilerpipe.Document;

namespace NBoilerpipe.Parser
{
    public class NBoilerpipeHtmlParser
    {
        private NBoilerpipeContentHandler contentHandler { get; set; }

        public NBoilerpipeHtmlParser(NBoilerpipeContentHandler contentHandler)
        {
            this.contentHandler = contentHandler;
        }

        public void Parse(String input)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(input);

            Traverse(htmlDocument.DocumentNode);
        }

        private void Traverse (HtmlNode node)
		{
			bool traverse = true;

			switch (node.NodeType) {
			case HtmlNodeType.Element:
				traverse = contentHandler.ElementNode (node);
				break;
			case HtmlNodeType.Text:
				traverse = contentHandler.TextNode ((HtmlTextNode)node);
				break;
			}

			if (node.HasChildNodes && traverse) {
				for (int i = 0; i < node.ChildNodes.Count; i++) 
					Traverse (node.ChildNodes [i]);

				if (node.NodeType == HtmlNodeType.Element)
					contentHandler.FlushBlock ();
			}
		}

        public TextDocument ToTextDocument()
        {
            return contentHandler.ToTextDocument();
        }
    }
}