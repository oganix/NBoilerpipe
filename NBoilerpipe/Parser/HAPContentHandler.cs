using System;
using HtmlAgilityPack;

namespace NBoilerpipe.Parser
{
    public interface HAPContentHandler
    {
        void ElementNode(HtmlNode node);
        void TextNode(HtmlTextNode node);
    }
}
