using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;

namespace SharedLibrary.Interfaces.Service
{
	public interface IEntityService<TEntity> where TEntity : class, IEntity<TEntity>
	{
		bool AreSiblings(TEntity one, TEntity two);
		bool AreConnected(TEntity current, TEntity ancestor);
		double GetConnectionRate(TEntity current, TEntity ancestor);
		double GetDegradation(TEntity mother, TEntity father);
		TEntity GetFirstAncestor(TEntity current, Genders gender);
		IEnumerable<TEntity> GetLowestCommonAncestors(TEntity one, TEntity two);
	}
}