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

		private Genders GetRandomGender()
		{
			return this.NumberGenerator.GetRandomDouble() > 0.5 ? Genders.Male : Genders.Female;
		}

		public Entity GetRandomEntity()
		{
			return new Entity {
				Age = Ages.Childhood,
				BornCycle = 0,
				Degeneration = 0,
				Generation = 0,
				Gender = this.GetRandomGender(),
				Longevity = this.NumberGenerator.GetDoubleAround(0.5),
				Pontency = this.NumberGenerator.GetDoubleAround(0.5),
			};
		}

		public Entity GetSuperEntity()
		{
			var ent = this.GetRandomEntity();

			// Set super modifiers
			ent.Pontency = 1;
			ent.Attractiveness = 1;
			ent.Longevity = 1;

			return ent;
		}

		private IEnumerable<Entity> GetRandomEnts(Func<IEnumerable<Entity>> func, int count)
		{
			if (func == null) {
				throw new ArgumentNullException(nameof(func));
			}
			if (count < 1) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			var malesCount = Convert.ToInt32(Math.Floor((double) count / 2));
			var femalesCount = count - malesCount;

			var males = func().Where(x => x.Gender == Genders.Male).Take(malesCount);
			var females = func().Where(x => x.Gender == Genders.Female).Take(femalesCount);

			return males.Concat(females);
		}

		public IEnumerable<Entity> GetRandomEntities(int count)
		{
			return this.GetRandomEnts(this.GetEnumerableEntities, count);
		}

		public IEnumerable<Entity> GetSuperEntities(int count)
		{
			return this.GetRandomEnts(this.GetEnumerableSuperEntities, count);
		}
	}
}