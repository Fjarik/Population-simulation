using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Core
{
	public class Engine<TEntity> : BaseEngine, IEngine<TEntity> where TEntity : class, IEntity<TEntity>
	{
		private INumberGenerator NumberGenerator { get; set; }
		private IEntityGenerator<TEntity> EntityGenerator { get; set; }

		public List<TEntity> Entities { get; set; }

		public int Cycle { get; set; }

		public Engine(INumberGenerator numberGenerator, IEntityGenerator<TEntity> entityGenerator)
		{
			this.NumberGenerator = numberGenerator;
			this.EntityGenerator = entityGenerator;
		}

		public void Configurate(List<TEntity> entities)
		{
			this.Entities = entities;
		}

		public void MakeBabies()
		{
			foreach (var mother in this.Entities.ChildrenMakeableFemales()) {
				this.MakeBabies(mother);
			}
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

			// Notify mother children about new sibling
			foreach (var entity in mother.Children) {
				entity.Siblings.Add(entity);
			}
		}

		public void MakeBabies(TEntity parent)
		{
			this.CheckEntity(parent);
			this.CheckEntity(parent.Partner);
			this.MakeBabies(parent, parent.Partner);
		}

		public void MakeBabies(TEntity father, TEntity mother)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			if (father.Gender == Genders.Female && mother.Gender == Genders.Male) {
				this.MakeBabies(mother, father);
				return;
			}
			var count = this.NumberGenerator.GetChildrenCount(father.Pontency, mother.Pontency);
			for (int i = 0; i < count; i++) {
				this.MakeBaby(father, mother);
			}
		}

		public void SetPartners()
		{
			foreach (var entity in this.Entities.SingleEntities(Ages.Adolescence)) {
				SetRandomPartner(entity);
			}
		}

		public void NextCycle()
		{
			this.GetOlder();
			this.SetPartners();
			this.Cycle++;
		}

		public void GetOlder()
		{
			foreach (var entity in this.Entities.LivingEntities()) {
				entity.LastAge = entity.Age;
				switch (entity.Age) {
					case Ages.Childhood:
						entity.Age = Ages.Adolescence;
						break;
					case Ages.Adolescence:
						entity.Age = Ages.Adulthood;
						break;
					case Ages.Adulthood:
						entity.Age = Ages.OldAge;
						break;
					case Ages.OldAge:
						this.Kill(entity);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void Kill(TEntity entity)
		{
			entity.DeathCycle = this.Cycle;
		}

		public void SetRandomPartner(TEntity original)
		{
			this.CheckEntity(original);
			if (original.Partner != null) {
				// Entity has a partner
				return;
			}
			var partner = this.GetPartner(original);
			if (partner != null) {
				partner.Partner = original;
			}
			original.Partner = partner;
		}

		public TEntity GetPartner(TEntity original)
		{
			this.CheckEntity(original);
			var minimalAtrac = this.NumberGenerator.GetRandomDouble();
			if (original.Attractiveness >= minimalAtrac) {
				return this.Entities.SingleEntities()
						   .FirstOrDefault(x => x.Gender != original.Gender &&
												x.Attractiveness >= minimalAtrac);
			}
			return null;
		}
	}
}