/*
 * This code is derived from boilerpipe
 * 
 */

using System.Collections.Generic;
using Sharpen;

namespace NBoilerpipe.Parser
{
	/// <summary>
	/// Base class for definition a set of
	/// <see cref="TagAction">TagAction</see>
	/// s that are to be used for the
	/// HTML parsing process.
	/// </summary>
	/// <seealso cref="DefaultTagActionMap">DefaultTagActionMap</seealso>
	/// <author>Christian Kohlsch√ºtter</author>
	[System.Serializable]
	public abstract class TagActionMap : Dictionary<string, TagAction>
	{
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Sets a particular
		/// <see cref="TagAction">TagAction</see>
		/// for a given tag. Any existing TagAction for that tag
		/// will be removed and overwritten.
		/// </summary>
		/// <param name="tag">The tag (will be stored internally 1. as it is, 2. lower-case, 3. upper-case)
		/// 	</param>
		/// <param name="action">
		/// The
		/// <see cref="TagAction">TagAction</see>
		/// </param>
		protected internal virtual void SetTagAction (string tag, TagAction action)
		{
			Add (tag.ToUpper (), action);
			Add (tag.ToLower (), action);
			if(!ContainsKey(tag)) Add(tag, action);
		}

		/// <summary>
		/// Adds a particular
		/// <see cref="TagAction">TagAction</see>
		/// for a given tag. If a TagAction already exists for that tag,
		/// a chained action, consisting of the previous and the new
		/// <see cref="TagAction">TagAction</see>
		/// is created.
		/// </summary>
		/// <param name="tag">The tag (will be stored internally 1. as it is, 2. lower-case, 3. upper-case)
		/// 	</param>
		/// <param name="action">
		/// The
		/// <see cref="TagAction">TagAction</see>
		/// </param>
		protected internal virtual void AddTagAction(string tag, TagAction action)
		{
			TagAction previousAction = this[tag];
			if (previousAction == null)
			{
				SetTagAction(tag, action);
			}
			else
			{
				SetTagAction(tag, new CommonTagActions.Chained(previousAction, action));
			}
		}
	}
}
