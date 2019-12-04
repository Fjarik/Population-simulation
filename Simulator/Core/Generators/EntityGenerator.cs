using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;
using SharedLibrary.Interfaces.Service;

namespace Core.Generators
{
	public class EntityGenerator<TEntity> : BaseEngine, IEntityGenerator<TEntity>
		where TEntity : class, IEntity<TEntity>, new()
	{
		private INumberGenerator NumberGenerator { get; }
		private IEntityService<TEntity> EntityService { get; }

		public EntityGenerator(INumberGenerator numberGenerator, IEntityService<TEntity> entityService)
		{
			this.NumberGenerator = numberGenerator;
			this.EntityService = entityService;
		}

		public Genders GetRandomGender()
		{
			return this.NumberGenerator.GetRandomDouble() > 0.5 ? Genders.Male : Genders.Female;
		}

		public TEntity GenerateBaby(TEntity father, TEntity mother, int cycle)
		{
			var degradation = this.EntityService.GetDegradation(mother, father);
			return this.GenerateBaby(father, mother, cycle, degradation);
		}

		private TEntity GenerateBaby(TEntity father, TEntity mother, int cycle, double degradation)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			this.CheckAge(father);
			this.CheckAge(mother);

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

			child.SetAncestors();

			father.Children.Add(child);
			mother.Children.Add(child);

			return child;
		}

		public TEntity GetRandomEntity(Ages? age = null)
		{
			return new TEntity {
				Id = Guid.NewGuid(),
				Age = age ?? Ages.Childhood,
				BornCycle = 0,
				Degeneration = 0,
				Generation = 0,
				Gender = this.GetRandomGender(),
				Longevity = this.NumberGenerator.GetDoubleAround(0.5),
				Pontency = this.NumberGenerator.GetDoubleAround(0.5),
				Attractiveness = this.NumberGenerator.GetDoubleAround(0.5),
			};
		}
	}
}