namespace Sharpen
{
	using System;

	public class CharBuffer : CharSequence
	{
		internal string Wrapped;

		public override string ToString ()
		{
			return Wrapped;
		}

		public static CharBuffer Wrap (string str)
		{
			CharBuffer buffer = new CharBuffer ();
			buffer.Wrapped = str;
			return buffer;
		}
	}
}
