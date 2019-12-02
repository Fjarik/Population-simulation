﻿using System;
using System.Collections.Generic;
using System.Text;
using SharedLibrary.Interfaces.Entity;

namespace SharedLibrary.Interfaces
{
	public interface IEngine<TEntity> where TEntity : class, IEntity
	{
		List<TEntity> Entities { get; set; }

		bool IsConfigurated { get; }
		int Cycle { get; set; }

		void Configurate(List<TEntity> entities);
		void Reset();
		void NextCycle();
		void GetOlder();
		void MakeBabies();
		void MakeBaby(TEntity parent);
		void MakeBaby(TEntity father, TEntity mother);
		void SetPartners();
		void Kill(TEntity entity);
		void SetRandomPartner(TEntity original);
	}
}