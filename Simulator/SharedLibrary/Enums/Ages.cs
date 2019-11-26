using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Enums
{
	/// <summary>
	/// Age phases of Entities. Child, Adolescence, Adult, Old
	/// </summary>
	public enum Ages : byte
	{
		/// <summary>
		/// The childhood - 0-12
		/// </summary>
		Childhood,

		/// <summary>
		/// The adolescence - 13-21
		/// </summary>
		Adolescence,

		/// <summary>
		/// The adulthood - 22-70
		/// </summary>
		Adulthood,

		/// <summary>
		/// The old age - 70-death
		/// </summary>
		OldAge,
	}
}