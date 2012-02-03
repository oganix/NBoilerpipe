namespace Sharpen
{
	using System;
	using System.Collections.Generic;

	public class ListIterator<T>
	{
		private IList<T> list;
		private int pos;

		public ListIterator (IList<T> list) : this(list,0)
		{
		}

		public ListIterator (IList<T> list, int n)
		{
			this.list = list;
			this.pos = n-1;
		}

		public bool HasPrevious ()
		{
			return (this.pos > 0);
		}
		public bool HasNext ()
		{
			return (this.pos + 1 < list.Count);
		}
		
		public T Next ()
		{	
			pos++;
			return list [pos];
		}

		public T Previous ()
		{
			pos--;
			return list [pos];
		}
		
		public void Remove ()
		{
			list.RemoveAt (pos);
			pos--;
		}
	}
}
