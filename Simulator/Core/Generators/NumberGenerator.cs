﻿using System;
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
			// max + 1 -> .Next(0,2) generates 0 or 1 but not 2!
			return this.GetRandom().Next(min, max + 1);
		}

		public int GetChildrenCount(double fatherPotency, double motherPotency)
		{
			var tolerance = 0.05;
			if (Math.Abs(fatherPotency) < tolerance || Math.Abs(motherPotency) < tolerance) {
				return 0;
			}
			var potency = fatherPotency + motherPotency / 2;
			return GetChildrenCount(potency);
		}

		public int GetChildrenCount(double potency)
		{
			if (potency < 0.05) {
				return 0;
			}
			var maxCount = 1;
			if (potency >= 0.15 && potency < 0.35) {
				maxCount = 2;
			} else if (potency >= 0.35 && potency < 0.55) {
				maxCount = 3;
			} else if (potency >= 0.55 && potency < 0.75) {
				maxCount = 4;
			} else if (potency >= 0.75) {
				maxCount = 5;
			}
			return this.GetRandomInt(1, maxCount);
		}

		private Random GetRandom()
		{
			Thread.Sleep(1);
			return new Random(DateTime.Now.Millisecond);
		}
	}
}