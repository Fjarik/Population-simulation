using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary.Interfaces.Statistics
{
	public interface IAgingStatistics
	{
		int NewTeens { get; }
		int NewAdult { get; }
		int NewOld { get; }
		int Deaths { get; }
	}
}