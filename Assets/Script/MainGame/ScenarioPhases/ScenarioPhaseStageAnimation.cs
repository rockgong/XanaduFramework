using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	public class ScenarioPhaseDataStageAnimatin : BaseScenarioPhaseData
	{
		public string targetName;
		public string animationName;
	}

	class ScenarioPhaseStageAnimatin : BaseScenarioPhase
	{
		public string targetName;
		public string animationName;

		public override void Enter()
		{
			IStage stage = _gameKernal.GetStage();
			if (stage != null && !string.IsNullOrEmpty(targetName) && !string.IsNullOrEmpty(animationName))
			{
				stage.PlayerStageAnimation(targetName, animationName);
			}
		}

		public override bool CheckOver()
		{
			return true;
		}

		public override void Exit()
		{
			return;
		}

		public static BaseScenarioPhase BuildHandler(BaseScenarioPhaseData data, ScenarioPhaseBuilder builder)
		{
			ScenarioPhaseDataStageAnimatin target = (ScenarioPhaseDataStageAnimatin)data;
			ScenarioPhaseStageAnimatin result = new ScenarioPhaseStageAnimatin();

			result.targetName = target.targetName;
			result.animationName = target.animationName;

			return result;
		}
	}
}