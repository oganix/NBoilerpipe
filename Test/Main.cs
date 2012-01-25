using System;
using System.IO;
using System.Net;
using System.Text;
using NBoilerpipe.Extractors;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/*
			String page = String.Empty;
			String url = "http://lyrics.wikia.com/Cake:Dime";
			WebRequest request = WebRequest.Create (url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			Stream stream = response.GetResponseStream ();
			
			using (StreamReader streamReader = new StreamReader (stream, Encoding.UTF8)) {
				page = streamReader.ReadToEnd ();
			}
			*/
			
			String page = File.ReadAllText ("/Users/aogan/Dropbox/Temp/simple.html");
			
			String lyrics = DefaultExtractor.INSTANCE.GetText (page);
			
			Console.WriteLine (lyrics);
		}
	}
}
