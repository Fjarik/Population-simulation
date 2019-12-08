using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Interfaces.Statistics;

namespace Core.Models
{
	public class CycleStatistics : ICycleStatistics
	{
		public int Births { get; }
		public int NewRelationships { get; }
		public IAgingStatistics AgingStats { get; }

		public CycleStatistics(int births, int newRelationships, IAgingStatistics agingStats)
		{
			this.Births = births;
			this.NewRelationships = newRelationships;
			this.AgingStats = agingStats;
		}
	}
}