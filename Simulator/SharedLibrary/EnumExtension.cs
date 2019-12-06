using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;

namespace SharedLibrary
{
	public static class EnumExtension
	{
		public static bool ChildrenMakeableAge(this Ages age, Genders gender)
		{
			switch (age) {
				case Ages.Adulthood:
				case Ages.OldAge when gender == Genders.Male:
					return true;
				default:
					return false;
			}
		}
	}
}