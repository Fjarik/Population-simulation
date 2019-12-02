using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Generators;
using Core.Models;
using NUnit.Framework;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Test
{
	public class EngineTests
	{
		private IEngine<Entity> _engine;
		private IGenerator<Entity> _generator;

		[SetUp]
		public void SetUp()
		{
			INumberGenerator numberGen = new NumberGenerator();
			IEntityGenerator<Entity> entityGenerator = new EntityGenerator<Entity>(numberGen);
			_generator = new StartGenerator(numberGen);
			_engine = new Engine<Entity>(numberGen, entityGenerator);

			var ent = _generator.GetRandomEntities(7).ToList();
			_engine.Configurate(ent);
		}

		[Test]
		public void GetSetPartner()
		{
			Assert.IsNotNull(_engine);

			var original = this._engine.Entities.SingleEntities().FirstOrDefault();

			Assert.IsNotNull(original);

			_engine.SetRandomPartner(original);

			if (original.Partner != null) {
				Assert.IsNotNull(original.Partner);
				return;
			}

			Assert.IsNull(original.Partner);
		}

		[Test]
		public void GetOlder()
		{
			Assert.IsNotNull(_engine);

			var allYoung = _engine.Entities.All(x => x.Age == Ages.Childhood);

			Assert.IsTrue(allYoung);

			Assert.AreEqual(0, _engine.Cycle);

			_engine.NextCycle();

			Assert.AreEqual(1, _engine.Cycle);

			var allTeen = _engine.Entities.All(x => x.Age == Ages.Adolescence);

			Assert.IsTrue(allTeen);
		}

		[Test]
		public void SetPartners()
		{
			Assert.IsNotNull(_engine);

			_engine.NextCycle();

			_engine.SetPartners();

			// TODO: Test
		}

		[Test]
		public void MakeBabies()
		{
			_engine.Reset();

			var ent = _generator.GetSuperEntities(4, Ages.Childhood).ToList();
			_engine.Configurate(ent);

			_engine.NextCycle();

			Assert.IsTrue(_engine.Entities.All(x => x.Age == Ages.Adolescence));
			Assert.IsTrue(_engine.Entities.All(x => x.IsSingle));
			Assert.IsTrue(_engine.Entities.All(x => !x.HasChildren));

			_engine.NextCycle();

			Assert.IsTrue(_engine.Entities.All(x => x.Age == Ages.Adulthood));
			Assert.IsTrue(_engine.Entities.All(x => !x.IsSingle));
			Assert.IsTrue(_engine.Entities.All(x => !x.HasChildren));

			_engine.NextCycle();

			Assert.IsTrue(_engine.Entities.MarriedEntities().All(x => x.HasChildren));
			_engine.NextCycle();
		}
	}
}