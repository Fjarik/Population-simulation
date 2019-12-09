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
	public class Engine<TEntity> : BaseEngine<TEntity>, IEngine<TEntity> where TEntity : class, IEntity<TEntity>
	{
		public Engine(INumberGenerator numberGenerator, IEntityGenerator<TEntity> entityGenerator) :
			base(numberGenerator, entityGenerator) { }

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
			if (!this.CanContinue) {
				this.Log("Engine cannot continue - No living entities");
				return new CycleStatistics();
			}

			var births = this.MakeBabies();
			var relations = this.SetPartners();
			var ageStats = this.GetOlder();
			this.Cycle++;

			this.Log($"Cycle no. {this.Cycle} done");
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
						if (entity.BornCycle != this.Cycle) {
							// NOT just born
							entity.Age = Ages.Adolescence;
							newTeens++;
						}
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

		public int SetRandomPartner(TEntity original)
		{
			this.CheckEntity(original);
			if (original.Partner != null) {
				// Entity has a partner
				return 0;
			}

			var sameGenOnly = this.GetSetting(SettingKeys.SameGenerationOnly, true);

			var partner = this.GetPartner(original, sameGenOnly);
			if (partner != null) {
				partner.Partner = original;
			}
			original.Partner = partner;
			return 1;
		}

		public TEntity GetPartner(TEntity original, bool sameGeneration)
		{
			this.CheckEntity(original);
			var minimalAtrac = this.NumberGenerator.GetRandomDouble();
			var query = this.Entities
							.SingleEntities();
			if (sameGeneration) {
				query = query.Where(x => x.Generation == original.Generation);
			}

			if (original.Attractiveness >= minimalAtrac) {
				return query
					.FirstOrDefault(x => x.Age == original.Age &&
										 x.Gender != original.Gender &&
										 x.Attractiveness >= minimalAtrac);
			}
			return null;
		}
	}
}