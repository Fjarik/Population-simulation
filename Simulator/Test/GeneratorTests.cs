using System.Linq;
using Core.Generators;
using Moq;
using NUnit.Framework;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Test
{
	public class GeneratorTests
	{
		public INumberGenerator NumberGenerator;
		public IGenerator<IEntity> Generator;

		[SetUp]
		public void Setup()
		{
			NumberGenerator = new NumberGenerator();
			Generator = new StartGenerator(this.NumberGenerator);
		}

		[Test]
		public void RendomGeneratorTest()
		{
			var count = 10;

			var entites = this.Generator.GetRandomEntities(count).ToList();
			var first = entites.Last();


			Assert.AreEqual(count, entites.Count);

			Assert.IsNotNull(first);

			Assert.AreEqual(0.5, first.Pontency, 0.5);
			Assert.AreEqual(0.5, first.Attractiveness, 0.5);
			Assert.AreEqual(0.5, first.Longevity, 0.5);

			Assert.IsNull(first.DeathCycle);

			Assert.Pass();
		}

		[Test]
		public void SuperGeneratorTest()
		{
			var count = 10;

			var entites = this.Generator.GetSuperEntities(count).ToList();
			var first = entites.Last();


			Assert.AreEqual(count, entites.Count);

			Assert.IsNotNull(first);

			Assert.AreEqual(1, first.Pontency);
			Assert.AreEqual(1, first.Attractiveness);
			Assert.AreEqual(1, first.Longevity);

			Assert.IsNull(first.DeathCycle);

			Assert.Pass();
		}
	}
}