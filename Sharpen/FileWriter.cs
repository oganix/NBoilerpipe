namespace Sharpen
{
	using System;
	using System.IO;

	public class FileWriter : StreamWriter
	{
		public FileWriter (FilePath path) : base(path.GetPath ())
		{
		}
	}
}
