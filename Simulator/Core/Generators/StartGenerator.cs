using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Generator;

namespace Core.Generators
{
	public sealed class StartGenerator : IGenerator<Entity>
	{
		public StartGenerator(INumberGenerator generator)
		{
			this.NumberGenerator = generator;
		}

		public INumberGenerator NumberGenerator { get; }

		private IEnumerable<Entity> GetEnumerableEntities()
		{
			while (true) {
				yield return GetRandomEntity();
			}
		}

		private IEnumerable<Entity> GetEnumerableSuperEntities()
		{
			while (true) {
				yield return GetSuperEntity();
			}
		}

		public Entity GetRandomEntity()
		{
			return new Entity {
				Age = Ages.Childhood,
				BornCycle = 0,
				Degeneration = 0,
				Generation = 0,
				Longevity = this.NumberGenerator.GetDoubleAround(0.5),
				Pontency = this.NumberGenerator.GetDoubleAround(0.5),
			};
		}

		public Entity GetSuperEntity()
		{
			return new Entity {
				Age = Ages.Childhood,
				BornCycle = 0,
				Degeneration = 0,
				Generation = 0,
				Pontency = 1,
				Attractiveness = 1,
				Longevity = 1
			};
		}

		public IEnumerable<Entity> GetRandomEntities(int count)
		{
			return GetEnumerableEntities().Take(count);
		}

		public IEnumerable<Entity> GetSuperEntities(int count)
		{
			return GetEnumerableSuperEntities().Take(count);
		}

		public IEnumerable<Entity> GetEntities(int count, Func<Entity, bool> where)
		{
			return GetEnumerableEntities().Where(where).Take(count);
		}
	}
}