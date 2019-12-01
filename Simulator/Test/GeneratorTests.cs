using System;
using System.Linq;
using Core.Generators;
using Core.Models;
using Moq;
using NUnit.Framework;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Test
{
	public class GeneratorTests
	{
		private INumberGenerator _numberGenerator;
		private IGenerator<Entity> _generator;
		private IEntityGenerator<Entity> _entGenerator;

		[SetUp]
		public void Setup()
		{
			_numberGenerator = new NumberGenerator();
			_generator = new StartGenerator(this._numberGenerator);
			_entGenerator = new EntityGenerator<Entity>();
		}

		[Test]
		public void RandomGeneratorTest()
		{
			Assert.IsNotNull(_generator);

			var count = 11;

			var entites = this._generator.GetRandomEntities(count).ToList();
			var first = entites.Last();


			Assert.AreEqual(count, entites.Count);

			var females = entites.Where(x => x.Gender == Genders.Female);
			var males = entites.Where(x => x.Gender == Genders.Male);

			var expectedMaleCount = Convert.ToInt32(Math.Floor((double) count / 2));
			var expectedFemaleCount = count - expectedMaleCount;

			Assert.AreEqual(expectedMaleCount, males.Count());
			Assert.AreEqual(expectedFemaleCount, females.Count());

			Assert.IsNotNull(first);

			Assert.AreEqual(0, first.Generation);
			Assert.AreEqual(0, first.BornCycle);

			Assert.AreEqual(0.5, first.Degeneration, 0.5);

			Assert.AreEqual(0.5, first.Pontency, 0.5);
			Assert.AreEqual(0.5, first.Attractiveness, 0.5);
			Assert.AreEqual(0.5, first.Longevity, 0.5);

			Assert.IsNull(first.DeathCycle);
		}

		[Test]
		public void SuperGeneratorTest()
		{
			Assert.IsNotNull(_generator);

			var count = 10;

			var entites = this._generator.GetSuperEntities(count).ToList();
			var first = entites.Last();


			Assert.AreEqual(count, entites.Count);

			var females = entites.Where(x => x.Gender == Genders.Female);
			var males = entites.Where(x => x.Gender == Genders.Male);

			var expectedMaleCount = Convert.ToInt32(Math.Floor((double) count / 2));
			var expectedFemaleCount = count - expectedMaleCount;

			Assert.AreEqual(expectedMaleCount, males.Count());
			Assert.AreEqual(expectedFemaleCount, females.Count());

			Assert.IsNotNull(first);

			Assert.AreEqual(0, first.Generation);
			Assert.AreEqual(0, first.BornCycle);

			Assert.AreEqual(0, first.Degeneration);

			Assert.AreEqual(1, first.Pontency);
			Assert.AreEqual(1, first.Attractiveness);
			Assert.AreEqual(1, first.Longevity);

			Assert.IsNull(first.DeathCycle);

			Assert.Pass();
		}

		[Test]
		public void GenderCountGeneratorTest()
		{
			Assert.IsNotNull(_numberGenerator);

			var total = 10;

			var males = _numberGenerator.GetMalesCount(total);
			var females = total - males;

			Assert.AreEqual(5, males);
			Assert.AreEqual(5, females);

			total = 11;

			males = _numberGenerator.GetMalesCount(total);
			females = total - males;

			Assert.AreEqual(5, males);
			Assert.AreEqual(6, females);
		}

		[Test]
		public void GenerateBaby()
		{
			Assert.IsNotNull(_generator);
			Assert.IsNotNull(_entGenerator);

			var count = 2;
			var cycle = 1;

			Assert.GreaterOrEqual(count, 2);

			var parents = _generator.GetSuperEntities(count, Ages.Adulthood).ToList();

			Assert.IsNotNull(parents);
			Assert.AreEqual(count, parents.Count);

			var mother = parents.FirstOrDefault(x => x.Gender == Genders.Female);
			var father = parents.FirstOrDefault(x => x.Gender == Genders.Male);

			Assert.IsNotNull(mother);
			Assert.IsNotNull(father);

			mother.Partner = father;
			father.Partner = mother;

			var baby = _entGenerator.GenerateBaby(father, father.Partner, cycle);

			Assert.IsNotNull(baby);

			Assert.AreEqual(Ages.Childhood, baby.Age);

			Assert.AreEqual(cycle, baby.BornCycle);

			Assert.IsTrue(baby.IsAlive);

			Assert.IsFalse(baby.Siblings.Any());

			Assert.AreEqual(mother, baby.Mother);
			Assert.AreEqual(father, baby.Father);
		}
	}
}