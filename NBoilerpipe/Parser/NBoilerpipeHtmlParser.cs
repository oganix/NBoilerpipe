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
		NBoilerpipeContentHandler contentHandler;

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
            if (node.NodeType == HtmlNodeType.Comment) { return; }

			if (node.NodeType == HtmlNodeType.Element) {
				contentHandler.StartElement (node);
			} else if (node.NodeType == HtmlNodeType.Text) {
				contentHandler.HandleText ((HtmlTextNode)node);
			}

			if (node.HasChildNodes) {
				for (int i = 0; i < node.ChildNodes.Count; i++) 
					Traverse (node.ChildNodes [i]);
			}
			
			if (node.NodeType == HtmlNodeType.Element)
				contentHandler.EndElement (node);
		}

        public TextDocument ToTextDocument()
        {
            return contentHandler.ToTextDocument();
        }
    }
}