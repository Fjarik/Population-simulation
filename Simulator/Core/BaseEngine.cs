using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Interfaces.Entity;
using SharedLibrary.Interfaces.Generator;

namespace Core
{
	public abstract class BaseEngine<TEntity> : BaseMethods where TEntity : class, IEntity<TEntity>
	{
		public INumberGenerator NumberGenerator { get; }
		public IEntityGenerator<TEntity> EntityGenerator { get; }

		private List<TEntity> _entities { get; set; }

		public IReadOnlyList<TEntity> Entities => _entities.AsReadOnly();
		private IDictionary<SettingKeys, object> _settings { get; }

		public IReadOnlyDictionary<SettingKeys, object> Settings =>
			new ReadOnlyDictionary<SettingKeys, object>(this._settings);

		public int Cycle { get; protected set; }
		public bool IsConfigurated { get; private set; }
		public bool LoggingEnabled { get; private set; }
		public bool CanContinue => this.Entities.LivingEntities().Any();

		protected BaseEngine(INumberGenerator numberGenerator, IEntityGenerator<TEntity> entityGenerator)
		{
			this.NumberGenerator = numberGenerator;
			this.EntityGenerator = entityGenerator;
			this._entities = new List<TEntity>();
			this._settings = new Dictionary<SettingKeys, object>();
		}

#region Methods

		public void Configurate(List<TEntity> entities)
		{
			if (this.IsConfigurated) {
				throw new ArgumentOutOfRangeException(nameof(this.IsConfigurated),
													  "Engine is already configurated. Use Reset() first.");
			}

			this._entities = entities;

			this._settings.Add(SettingKeys.AllowIncest, true);
			this._settings.Add(SettingKeys.MinRelationDegree, 1);
			this._settings.Add(SettingKeys.SameAgeOnly, true);
			this._settings.Add(SettingKeys.SameGenerationOnly, true);
			this._settings.Add(SettingKeys.RandomDeaths, true);

			this.IsConfigurated = true;

			this.Log("Engine configurated");
		}

		public void Reset()
		{
			if (!this.IsConfigurated) {
				throw new ArgumentOutOfRangeException(nameof(this.IsConfigurated), "Engine is not configurated");
			}

			this._entities.Clear();
			this._settings.Clear();
			this.Cycle = 0;

			this.IsConfigurated = false;
			this.LoggingEnabled = false;

			this.Log("Engine reset");
		}

		public void ToggleLogging()
		{
			this.Log("Logging toggled");
			this.LoggingEnabled = !this.LoggingEnabled;
		}

		protected void Log(string text)
		{
			if (!this.LoggingEnabled) {
				return;
			}
			if (string.IsNullOrWhiteSpace(text)) {
				Console.WriteLine("Log text is empty");
				return;
			}
			Console.WriteLine(text);
		}

#region Settings

		public void SetSetting(SettingKeys key, object value)
		{
			this.Log($"Setting setting: Key = '{key}'; Value = '{value ?? "null"}'");
			if (value == null) {
				return;
			}

			if (this._settings.ContainsKey(key)) {
				this._settings[key] = value;
				return;
			}
			this._settings.Add(key, value);
		}

		public T GetSetting<T>(SettingKeys key, T defaultValue)
		{
			if (!this.Settings.ContainsKey(key)) {
				return defaultValue;
			}

			var obj = this.Settings[key];

			if (obj is T res) {
				return res;
			}

			try {
				return (T) Convert.ChangeType(obj, typeof(T));
			} catch {
				return defaultValue;
			}
		}

		public void RemoveSetting(SettingKeys key)
		{
			this._settings.Remove(key);
			this.Log($"Removed setting: Key = '{key}'");
		}

#endregion

#region Entites

		public virtual void MakeBaby(TEntity father, TEntity mother)
		{
			this.CheckEntity(father);
			this.CheckEntity(mother);
			if (father.Gender != Genders.Male) {
				throw new InvalidCastException("Father has wrong gender");
			}
			if (mother.Gender != Genders.Female) {
				throw new InvalidCastException("Mother has wrong gender");
			}
			var child = this.EntityGenerator.GenerateBaby(father, mother, this.Cycle);
			this.CheckEntity(child);

			this._entities.Add(child);
		}

		public virtual void Kill(TEntity entity)
		{
			entity.DeathCycle = this.Cycle;
			this.Log($"Entity '{entity.Id}' killed");
		}

#endregion

#endregion
	}
}