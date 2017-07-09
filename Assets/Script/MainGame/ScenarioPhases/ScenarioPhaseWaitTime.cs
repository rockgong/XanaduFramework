using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class ScenarioPhaseDataWaitTime : BaseScenarioPhaseData
	{
		public float time;
	}

	class ScenarioPhaseWaitTime : BaseScenarioPhase
	{
		public float time;
		private float _timeCnt;

		public override void Enter()
		{
			_timeCnt = time;

			return;
		}

		public override bool CheckOver()
		{
			_timeCnt -= Time.deltaTime;
			if (_timeCnt <= 0.0f)
				return true;
			return false;
		}
		public override void Exit()
		{
			return;
		}

		public static BaseScenarioPhase BuildHandler(BaseScenarioPhaseData data, ScenarioPhaseBuilder builder)
		{
			ScenarioPhaseDataWaitTime target = (ScenarioPhaseDataWaitTime)data;
			ScenarioPhaseWaitTime result = new ScenarioPhaseWaitTime();

			result.time = target.time;

			return result;
		}
	}
}