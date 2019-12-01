using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Generator
{
	public interface INumberGenerator
	{
		double GetRandomDouble();
		double GetDoubleAround(double around, double precision = 0.1);
		double GetRandomDouble(double min, double max);
		int GetRandomInt();
		int GetRandomInt(int max);
		int GetRandomInt(int min, int max);
		int GetMalesCount(int total);
		int GetChildrenCount(double fatherPotency, double motherPotency);
		int GetChildrenCount(double potency);
	}
}