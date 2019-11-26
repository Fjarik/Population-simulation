using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;

namespace SharedLibrary.Interfaces
{
	public interface IGenerator<T> where T : class, IEntity
	{
		T GetRandomEntity();

		/// <summary>
		/// Gets the super entity = MAX all modifiers.
		/// </summary>
		/// <returns>Returns the Entity</returns>
		T GetSuperEntity();

		List<T> GetRandomEntities(int count);
		List<T> GetSuperEntities(int count);
	}
}