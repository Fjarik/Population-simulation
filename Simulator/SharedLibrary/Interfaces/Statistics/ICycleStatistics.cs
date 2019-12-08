using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Statistics
{
	public interface ICycleStatistics
	{
		int Births { get; }
		int NewRelationships { get; }
		IAgingStatistics AgingStats { get; }
	}
}