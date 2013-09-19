NBoilerpipe
===========

NBoilerpipe was a C# port of the java-based Boilerpipe project. I've forked it from [Organix/NBoilerpipe](https://github.com/oganix/NBoilerpipe) and
removed the Mono and SharpZipLib dependencies.

This project now builds in Visual Studio 2012 and should be useful as a drop-in library.


The following is the README from Organix/NBoilerpipe:
-----------------------------------------------------

NBoilerpipe is a C# port of boilerpipe 1.2 (http://code.google.com/p/boilerpipe/) library.  Most of the code is converted  with the Sharpen tool (https://github.com/slluis/sharpen). The code uses the Sharpen libary (with modification) from NGit project (https://github.com/slluis/ngit) and HmtlAgilityPack (http://htmlagilitypack.codeplex.com/). 

NBoilerpipe is only been tested with Mono. 

Usage:

using NBoilerpipe.Extractors;  
...  
String html = GetHtmlText();  
var text = ArticleExtractor.INSTANCE.GetText (html);  
//var text = DefaultExtractor.INSTANCE.GetText (html);   
...