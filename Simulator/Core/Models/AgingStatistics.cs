using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Interfaces.Statistics;

namespace Core.Models
{
	public class AgingStatistics : IAgingStatistics
	{
		public int NewTeens { get; }
		public int NewAdult { get; }
		public int NewOld { get; }
		public int Deaths { get; }

		public AgingStatistics(int newTeens, int newAdult, int newOld, int deaths)
		{
			NewTeens = newTeens;
			NewAdult = newAdult;
			NewOld = newOld;
			Deaths = deaths;
		}
	}
}