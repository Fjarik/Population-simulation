using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;

namespace SharedLibrary.Interfaces.Entity
{
	public interface IEntity : IEntityModifiers, IEntityRelationships
	{
		Genders Gender { get; set; }
		Ages Age { get; set; }
		int BornCycle { get; set; }
		int? DeathCycle { get; set; }
		int Generation { get; set; }
		double Degeneration { get; set; }
	}
}