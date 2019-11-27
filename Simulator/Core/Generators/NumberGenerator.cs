using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SharedLibrary.Interfaces.Generator;

namespace Core.Generators
{
	public sealed class NumberGenerator : INumberGenerator
	{
		public double GetRandomDouble()
		{
			return this.GetRandom().NextDouble();
		}

		public double GetRandomDouble(double min, double max)
		{
			return this.GetRandomDouble() * (max - min) + min;
		}

		public double GetDoubleAround(double around, double precision = 0.1)
		{
			return this.GetRandomDouble(around - precision, around + precision);
		}

		public int GetRandomInt()
		{
			return this.GetRandom().Next();
		}

		public int GetRandomInt(int max)
		{
			return this.GetRandom().Next(max);
		}

		public int GetRandomInt(int min, int max)
		{
			return this.GetRandom().Next(min, max);
		}

		private Random GetRandom()
		{
			Thread.Sleep(1);
			return new Random(DateTime.Now.Millisecond);
		}
	}
}