using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Core.Generators
{
	public class EntityGenerator<TEntity> : BaseEngine, IEntityGenerator<TEntity>
		where TEntity : class, IEntity<TEntity>, new()
	{
		public TEntity GenerateBaby(TEntity father, TEntity mother, int cycle)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			this.CheckAge(father);
			this.CheckAge(mother);

			var child = new TEntity() {
				Age = Ages.Childhood,
				BornCycle = cycle,
				Father = father,
				Mother = mother,
			};

			father.Children.Add(child);
			mother.Children.Add(child);

			return child;
		}

		private void CheckAge(TEntity entity)
		{
			if (entity.Age != Ages.Adulthood) {
				throw new ArgumentOutOfRangeException(nameof(entity), "Entity is not Adult");
			}
		}
	}
}