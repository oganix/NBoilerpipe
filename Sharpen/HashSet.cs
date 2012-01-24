using System;

namespace Sharpen
{
	public class HashSet<T>
	{
		System.Collections.Generic.HashSet<T> hashSet;
		
		public HashSet (int capacity)
		{
			hashSet = new System.Collections.Generic.HashSet<T>();
		}
		
		public bool Contains (T item)
		{
			return hashSet.Contains (item);
		}
		
		public bool Remove (T item)
		{
			return hashSet.Remove (item);
		}
		
		public bool AddItem (T item)
		{
			return hashSet.Add (item);
		}
	}
}

