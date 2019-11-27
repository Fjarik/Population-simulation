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

		T GetRandomEntity();

		/// <summary>
		/// Gets the super entity = MAX all modifiers.
		/// </summary>
		/// <returns>Returns the Entity</returns>
		T GetSuperEntity();

		IEnumerable<T> GetRandomEntities(int count);
		IEnumerable<T> GetSuperEntities(int count);
		IEnumerable<T> GetEntities(int count, Func<T, bool> where);
	}
}