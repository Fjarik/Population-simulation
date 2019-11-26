using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Entity
{
	public interface IEntityRelationships
	{
		IEntity Mother { get; set; }
		IEntity Father { get; set; }
		List<IEntity> Siblings { get; set; }
		IEntity Partner { get; set; }
		List<IEntity> Children { get; set; }
	}
}