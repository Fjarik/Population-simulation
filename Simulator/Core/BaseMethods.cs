using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;

namespace Core
{
	public abstract class BaseMethods
	{
		protected void CheckEntity(IEntity entity, bool onlyAlive = true)
		{
			if (entity == null) {
				throw new ArgumentNullException(nameof(entity));
			}
			if (onlyAlive && !entity.IsAlive) {
				throw new ArgumentOutOfRangeException(nameof(entity), "Entity is dead");
			}
		}

		protected void CheckAge(IEntity entity, bool allowOlder = false)
		{
			switch (entity.Age) {
				case Ages.Adulthood:
				case Ages.OldAge when allowOlder:
					return;
				default:
					throw new ArgumentOutOfRangeException(nameof(entity), "Entity is not Adult");
			}
		}

		protected void CheckParents<TEntity>(IEntity<TEntity> entity, bool onlyAlive = false)
			where TEntity : class, IEntity<TEntity>
		{
			this.CheckEntity(entity, onlyAlive);
			this.CheckEntity(entity.Mother, onlyAlive);
			this.CheckEntity(entity.Father, onlyAlive);
		}
	}
}