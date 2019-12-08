using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Generators;
using Core.Models;
using Core.Services;
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
		private EntityService<Entity> _entityService;

		[SetUp]
		public void SetUp()
		{
			var numberGen = new NumberGenerator();
			_entityService = new EntityService<Entity>();

			IEntityGenerator<Entity> entityGenerator = new EntityGenerator<Entity>(numberGen, _entityService);
			_generator = new StartGenerator(numberGen, entityGenerator);
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

		[Test]
		public void ServiceTest()
		{
			_engine.Reset();

			var ent = _generator.GetRandomEntities(7).ToList();
			_engine.Configurate(ent);

			var cycles = 10;
			for (int i = 0; i < cycles; i++) {
				_engine.NextCycle();
			}

			var last = _engine.Entities.Last();

			var firstAncestor = _entityService.GetFirstAncestor(last, Genders.Male);

			Assert.IsNotNull(firstAncestor);

			var siblings = _engine.Entities.LivingEntities().FirstOrDefault(x => x.Siblings.Any())?.Siblings;

			Assert.IsNotNull(siblings);
			Assert.IsTrue(siblings.Any());

			var areSibling = _entityService.AreSiblings(siblings.First(), siblings.Last());

			Assert.IsTrue(areSibling);
		}

		[Test]
		public void MultipleNextTest()
		{
			var count = 10;
			for (int i = 0; i < count; i++) {
				var cycles = 20;

				_engine.Reset();
				var ent = _generator.GetSuperEntities(5, Ages.Childhood).ToList();
				_engine.Configurate(ent);

				for (int j = 0; j < cycles; j++) {
					_engine.NextCycle();
				}

				var entCount = _engine.Entities.Count;
				var aliveCount = _engine.Entities.LivingEntities().Count();
				Console.WriteLine($"Try no. {i + 1}, after {cycles} cycles - Entities count: {entCount} (Alive: {aliveCount})");
			}

			Assert.IsTrue(true);
		}

		[Test]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		public void NextTest(int startCount)
		{
			var cycles = 50;

			_engine.Reset();
			var ent = _generator.GetSuperEntities(startCount, Ages.Childhood).ToList();
			_engine.Configurate(ent);

			for (int i = 0; i < cycles; i++) {
				Assert.AreEqual(i, _engine.Cycle);

				var stats = _engine.NextCycle();
				if (!stats.CanContinue) {
					Console.WriteLine("Cannot continue");
					break;
				}

				var entCount = _engine.Entities.Count;
				var aliveCount = _engine.Entities.LivingEntities().Count();
				Console.WriteLine($"Cycle no. {_engine.Cycle}, Entities count: {entCount} (Alive: {aliveCount})");
				Console.WriteLine($"	Births:	{stats.Births}");
				Console.WriteLine($"	New relationships: {stats.NewRelationships}");
				Console.WriteLine($"	New teens: {stats.AgingStats.NewTeens}");
				Console.WriteLine($"	New adults: {stats.AgingStats.NewAdult}");
				Console.WriteLine($"	New old(s): {stats.AgingStats.NewOld}");
				Console.WriteLine($"	Deaths: {stats.AgingStats.Deaths}");
			}
		}
	}
}