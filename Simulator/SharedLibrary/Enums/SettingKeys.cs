using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Enums
{
	public enum SettingKeys
	{
		/// <summary>
		/// 
		/// </summary>
		AllowIncest,

		/// <summary>
		/// The minimum relation degree
		/// https://en.wikipedia.org/wiki/File:Table_of_Consanguinity_showing_degrees_of_relationship.svg		
		/// </summary>
		MinRelationDegree,

		/// <summary>
		/// The same generation only
		/// </summary>
		SameGenerationOnly,

		/// <summary>
		/// Allow random deaths
		/// </summary>
		RandomDeaths,
	}
}