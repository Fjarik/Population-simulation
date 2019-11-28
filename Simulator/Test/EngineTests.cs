using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Generators;
using Core.Models;
using NUnit.Framework;
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
			IEntityGenerator<Entity> entityGenerator; // TODO: Implement
			_generator = new StartGenerator(numberGen);
			_engine = new Engine<Entity>(numberGen, entityGenerator);

			var ent = _generator.GetRandomEntities(7).ToList();
			_engine.Configurate(ent);
		}

		[Test]
		public void GetSetPartner()
		{
			Assert.IsNotNull(_engine);

			var original = this._engine.SingleLivingEntities.FirstOrDefault();

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
	}
}