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
		private IEntityGenerator<Entity> EntityGenerator { get; }
		public INumberGenerator NumberGenerator { get; }

		public StartGenerator(INumberGenerator generator, IEntityGenerator<Entity> entityGenerator)
		{
			this.NumberGenerator = generator;
			this.EntityGenerator = entityGenerator;
		}

		private IEnumerable<Entity> GetEnumerableEntities(Ages age)
		{
			while (true) {
				yield return this.EntityGenerator.GetRandomEntity(age);
			}
		}

		private IEnumerable<Entity> GetEnumerableSuperEntities(Ages age)
		{
			while (true) {
				yield return GetSuperEntity(age);
			}
		}

		public Entity GetSuperEntity(Ages? age = null)
		{
			var ent = this.EntityGenerator.GetRandomEntity(age);

			// Set super modifiers
			ent.Pontency = 1;
			ent.Attractiveness = 1;
			ent.Longevity = 1;

			return ent;
		}

		private IEnumerable<Entity> GetRandomEnts(Func<Ages, IEnumerable<Entity>> func, int count, Ages age)
		{
			if (func == null) {
				throw new ArgumentNullException(nameof(func));
			}
			if (count < 1) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			var malesCount = this.NumberGenerator.GetMalesCount(count);
			var femalesCount = count - malesCount;

			var males = func(age).Where(x => x.Gender == Genders.Male).Take(malesCount);
			var females = func(age).Where(x => x.Gender == Genders.Female).Take(femalesCount);

			return males.Concat(females);
		}

		public IEnumerable<Entity> GetRandomEntities(int count, Ages age = Ages.Childhood)
		{
			return this.GetRandomEnts(this.GetEnumerableEntities, count, age);
		}

		public IEnumerable<Entity> GetSuperEntities(int count, Ages age = Ages.Childhood)
		{
			return this.GetRandomEnts(this.GetEnumerableSuperEntities, count, age);
		}
	}
}