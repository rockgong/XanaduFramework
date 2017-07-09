using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public interface BaseScenarioPhaseData
	{

	}

	public interface IScenarioPhaseDatabase
	{
		BaseScenarioPhaseData GetDataById(int id);
	}

	class ScenarioPhaseManager
	{
		private IScenarioPhaseDatabase _database;
		private ScenarioPhaseBuilder _builder;

		public void Initialize(IScenarioPhaseDatabase db, ScenarioPhaseBuilder builder)
		{
			_database = db;
			_builder = builder;
		}

		public BaseScenarioPhase GetPhaseById(int id)
		{
			BaseScenarioPhaseData data = _database.GetDataById(id);
			if (data != null)
				return _builder.Build(data);
			return null;
		}
	}
}