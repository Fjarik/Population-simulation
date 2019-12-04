using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;

namespace SharedLibrary.Interfaces.Entity
{
	public interface IEntity : IEntityModifiers
	{
		bool IsAlive { get; }
		bool IsSingle { get; }
		bool HasChildren { get; }

		Guid Id { get; set; }
		Genders Gender { get; set; }
		Ages LastAge { get; set; }
		Ages Age { get; set; }
		int BornCycle { get; set; }
		int? DeathCycle { get; set; }
		int Generation { get; set; }
		double Degeneration { get; set; }
	}

	public interface IEntity<TEntity> : IEntity, IEntityRelationships<TEntity> where TEntity : class, IEntity
	{
		List<TEntity> Ancestors { get; }
		void SetAncestors();
	}
}