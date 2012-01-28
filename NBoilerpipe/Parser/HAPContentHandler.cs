using System;
using HtmlAgilityPack;

namespace NBoilerpipe.Parser
{
    public interface IContentHandler
    {
        bool ElementNode(HtmlNode node);
        bool TextNode(HtmlTextNode node);
    }
}
