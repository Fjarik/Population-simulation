using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Entity
{
	public interface IEntityModifiers
	{
		double Attractiveness { get; set; }

		double Pontency { get; set; }

		double Longevity { get; set; }
	}
}