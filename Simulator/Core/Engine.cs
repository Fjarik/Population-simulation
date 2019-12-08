using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Models;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;
using SharedLibrary.Interfaces.Statistics;

namespace Core
{
	public class Engine<TEntity> : BaseEngine, IEngine<TEntity> where TEntity : class, IEntity<TEntity>
	{
		private INumberGenerator NumberGenerator { get; }
		private IEntityGenerator<TEntity> EntityGenerator { get; }

		public List<TEntity> Entities { get; set; }
		public bool IsConfigurated { get; private set; }
		public int Cycle { get; set; }

		public Engine(INumberGenerator numberGenerator, IEntityGenerator<TEntity> entityGenerator)
		{
			this.NumberGenerator = numberGenerator;
			this.EntityGenerator = entityGenerator;
		}

		public void Configurate(List<TEntity> entities)
		{
			if (this.IsConfigurated) {
				throw new ArgumentOutOfRangeException(nameof(this.IsConfigurated),
													  "Engine is already configurated. Use Reset() first.");
			}

			this.Entities = entities;

			this.IsConfigurated = true;
		}

		public void Reset()
		{
			if (!this.IsConfigurated) {
				throw new ArgumentOutOfRangeException(nameof(this.IsConfigurated), "Engine is not configurated");
			}

			this.Entities.Clear();
			this.Cycle = 0;

			this.IsConfigurated = false;
		}

		public int MakeBabies()
		{
			var availableMothers = this.Entities.ChildrenMakeableFemales().ToList();
			return availableMothers.Sum(this.MakeBabies);
		}

		public void MakeBaby(TEntity parent)
		{
			this.CheckEntity(parent);
			this.CheckEntity(parent.Partner);
			this.MakeBaby(parent, parent.Partner);
		}

		public void MakeBaby(TEntity father, TEntity mother)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			if (father.Gender != Genders.Male) {
				throw new InvalidCastException("Father has wrong gender");
			}
			if (mother.Gender != Genders.Female) {
				throw new InvalidCastException("Mother has wrong gender");
			}
			var child = this.EntityGenerator.GenerateBaby(father, mother, this.Cycle);
			this.CheckEntity(child);

			this.Entities.Add(child);
		}

		public int MakeBabies(TEntity parent)
		{
			this.CheckEntity(parent);
			this.CheckEntity(parent.Partner);
			return this.MakeBabies(parent, parent.Partner);
		}

		public int MakeBabies(TEntity father, TEntity mother)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			if (father.Gender == Genders.Female && mother.Gender == Genders.Male) {
				return this.MakeBabies(mother, father);
			}
			var count = this.NumberGenerator.GetChildrenCount(father.Pontency, mother.Pontency);
			for (int i = 0; i < count; i++) {
				this.MakeBaby(father, mother);
			}
			return count;
		}

		public int SetPartners()
		{
			return this.Entities.SingleEntities(Ages.Adolescence).Sum(SetRandomPartner);
		}

		public ICycleStatistics NextCycle()
		{
			var births = this.MakeBabies();
			var relations = this.SetPartners();
			var ageStats = this.GetOlder();
			this.Cycle++;

			return new CycleStatistics(births, relations, ageStats);
		}

		public IAgingStatistics GetOlder()
		{
			var newTeens = 0;
			var newAdult = 0;
			var newOld = 0;
			var deaths = 0;
			foreach (var entity in this.Entities.LivingEntities()) {
				var minimum = this.NumberGenerator.GetRandomDouble(0, 0.9);
				var shouldGetOlder = (entity.Age == Ages.Adulthood && entity.LastAge == Ages.Adulthood) ||
									 entity.Age == Ages.Adolescence ||
									 entity.Longevity <= minimum;

				entity.LastAge = entity.Age;
				switch (entity.Age) {
					case Ages.Childhood:
						entity.Age = Ages.Adolescence;
						newTeens++;
						break;
					case Ages.Adolescence:
						entity.Age = Ages.Adulthood;
						newAdult++;
						break;
					case Ages.Adulthood:
						if (shouldGetOlder) {
							entity.Age = Ages.OldAge;
							newOld++;
						}
						break;
					case Ages.OldAge:
						this.Kill(entity);
						deaths++;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return new AgingStatistics(newTeens, newAdult, newOld, deaths);
		}

		public void Kill(TEntity entity)
		{
			entity.DeathCycle = this.Cycle;
		}

		public int SetRandomPartner(TEntity original)
		{
			this.CheckEntity(original);
			if (original.Partner != null) {
				// Entity has a partner
				return 0;
			}
			var partner = this.GetPartner(original);
			if (partner != null) {
				partner.Partner = original;
			}
			original.Partner = partner;
			return 1;
		}

		public TEntity GetPartner(TEntity original)
		{
			this.CheckEntity(original);
			var minimalAtrac = this.NumberGenerator.GetRandomDouble();
			if (original.Attractiveness >= minimalAtrac) {
				return this.Entities
						   .SingleEntities()
						   .FirstOrDefault(x => x.Generation == original.Generation &&
												x.Age == original.Age &&
												x.Gender != original.Gender &&
												x.Attractiveness >= minimalAtrac);
			}
			return null;
		}
	}
}