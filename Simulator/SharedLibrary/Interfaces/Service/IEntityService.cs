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
		int GetRelationDegree(TEntity one, TEntity two);
		int GetRelationDegree(int genOne, int genTwo, int genAncestor);
		int GetRelationDegree(int genChild, int genAncestor);
		double GetDegradation(TEntity mother, TEntity father);
		TEntity GetFirstAncestor(TEntity current, Genders gender);
		IEnumerable<TEntity> GetLowestCommonAncestors(TEntity one, TEntity two);
	}
}