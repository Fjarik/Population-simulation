using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;

namespace SharedLibrary
{
	public static class QueryExtensions
	{
		public static IEnumerable<TEntity> LivingEntities<TEntity>(this IEnumerable<TEntity> query)
			where TEntity : class, IEntity<TEntity>
		{
			return query.Where(x => x.IsAlive);
		}

		public static IEnumerable<TEntity> MarriedEntities<TEntity>(this IEnumerable<TEntity> query)
			where TEntity : class, IEntity<TEntity>
		{
			return query.LivingEntities().Where(x => !x.IsSingle);
		}

		public static IEnumerable<TEntity> SingleEntities<TEntity>(this IEnumerable<TEntity> query, Ages? age = null)
			where TEntity : class, IEntity<TEntity>
		{
			var res = query.LivingEntities().Where(x => x.IsSingle);
			if (age != null) {
				res = res.Where(x => x.Age == age);
			}
			return res;
		}

		public static IEnumerable<TEntity> ChildrenMakeableEntities<TEntity>(this IEnumerable<TEntity> query)
			where TEntity : class, IEntity<TEntity>
		{
			return query.MarriedEntities().Where(x => x.Partner.IsAlive &&
													  x.Age == Ages.Adulthood);
		}

		public static IEnumerable<TEntity> ChildrenMakeableFemales<TEntity>(this IEnumerable<TEntity> query)
			where TEntity : class, IEntity<TEntity>
		{
			return query.ChildrenMakeableEntities().Where(x => x.Gender == Genders.Female);
		}
	}
}