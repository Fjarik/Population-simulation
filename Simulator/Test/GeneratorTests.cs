using System;
using System.Linq;
using Core.Generators;
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
		private IGenerator<IEntity> _generator;

		[SetUp]
		public void Setup()
		{
			_numberGenerator = new NumberGenerator();
			_generator = new StartGenerator(this._numberGenerator);
		}

		[Test]
		public void RendomGeneratorTest()
		{
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
	}
}