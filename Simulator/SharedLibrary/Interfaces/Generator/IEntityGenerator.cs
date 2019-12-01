using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Interfaces.Entity;

namespace SharedLibrary.Interfaces.Generator
{
	public interface IEntityGenerator<TEntity> where TEntity : class, IEntity
	{
		TEntity GenerateBaby(TEntity father, TEntity mother, int cycle);
	}
}