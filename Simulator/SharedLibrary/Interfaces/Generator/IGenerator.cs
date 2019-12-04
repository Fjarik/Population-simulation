using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace SharedLibrary.Interfaces.Generator
{
	public interface IGenerator<out T> where T : class, IEntity
	{
		INumberGenerator NumberGenerator { get; }

		/// <summary>
		/// Gets the super entity = MAX all modifiers.
		/// </summary>
		/// <returns>Returns the Entity</returns>
		T GetSuperEntity(Ages? age = null);

		IEnumerable<T> GetRandomEntities(int count, Ages age = Ages.Childhood);
		IEnumerable<T> GetSuperEntities(int count, Ages age = Ages.Childhood);
	}
}