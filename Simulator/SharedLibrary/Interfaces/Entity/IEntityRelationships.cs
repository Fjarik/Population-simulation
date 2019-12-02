using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Entity
{
	public interface IEntityRelationships<TEntity> where TEntity : IEntity
	{
		TEntity Mother { get; set; }
		TEntity Father { get; set; }
		List<TEntity> Siblings { get; }
		TEntity Partner { get; set; }
		List<TEntity> Children { get; set; }
	}
}