using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Interfaces.Entity;

namespace Core
{
	public abstract class BaseEngine
	{
		protected void CheckEntity(IEntity entity)
		{
			if (entity == null) {
				throw new ArgumentNullException(nameof(entity));
			}
		}
	}
}