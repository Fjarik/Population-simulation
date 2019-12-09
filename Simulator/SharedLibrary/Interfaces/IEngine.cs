using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;
using SharedLibrary.Interfaces.Statistics;

namespace SharedLibrary.Interfaces
{
	public interface IEngine<TEntity> where TEntity : class, IEntity
	{
#region In ctor

		INumberGenerator NumberGenerator { get; }
		IEntityGenerator<TEntity> EntityGenerator { get; }

#endregion

		IReadOnlyList<TEntity> Entities { get; }
		IReadOnlyDictionary<SettingKeys, object> Settings { get; }

		int Cycle { get; }
		bool IsConfigurated { get; }
		bool CanContinue { get; }
		bool LoggingEnabled { get; }

		void Configurate(List<TEntity> entities);
		void Reset();
		ICycleStatistics NextCycle();
		IAgingStatistics GetOlder();
		int MakeBabies();
		void MakeBaby(TEntity parent);
		void MakeBaby(TEntity father, TEntity mother);
		int SetPartners();
		void Kill(TEntity entity);
		int SetRandomPartner(TEntity original);

		void ToggleLogging();

		void SetSetting(SettingKeys key, object value);
		T GetSetting<T>(SettingKeys key, T defaultValue);
		void RemoveSetting(SettingKeys key);
	}
}