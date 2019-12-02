using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Core.Generators
{
	public class EntityGenerator<TEntity> : BaseEngine, IEntityGenerator<TEntity>
		where TEntity : class, IEntity<TEntity>, new()
	{
		private INumberGenerator NumberGenerator { get; }

		public EntityGenerator(INumberGenerator numberGenerator)
		{
			this.NumberGenerator = numberGenerator;
		}

		public Genders GetRandomGender()
		{
			return this.NumberGenerator.GetRandomDouble() > 0.5 ? Genders.Male : Genders.Female;
		}

		public TEntity GenerateBaby(TEntity father, TEntity mother, int cycle)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			this.CheckAge(father);
			this.CheckAge(mother);

			var degradation = this.NumberGenerator.CalculateDegradation(mother.Degeneration, father.Degeneration);

			var child = new TEntity() {
				Id = Guid.NewGuid(),
				Age = Ages.Childhood,
				BornCycle = cycle,
				Father = father,
				Mother = mother,
				Generation = mother.Generation + 1,
				Gender = this.GetRandomGender(),
				Attractiveness =
					this.NumberGenerator.CalculateModifier(mother.Attractiveness, father.Attractiveness, degradation),
				Longevity = this.NumberGenerator.CalculateModifier(mother.Longevity, father.Longevity, degradation),
				Pontency = this.NumberGenerator.CalculateModifier(mother.Pontency, father.Pontency, degradation),
				Degeneration = degradation,
			};

			father.Children.Add(child);
			mother.Children.Add(child);

			return child;
		}

		private void CheckAge(TEntity entity)
		{
			if (entity.Age != Ages.Adulthood) {
				throw new ArgumentOutOfRangeException(nameof(entity), "Entity is not Adult");
			}
		}
	}
}