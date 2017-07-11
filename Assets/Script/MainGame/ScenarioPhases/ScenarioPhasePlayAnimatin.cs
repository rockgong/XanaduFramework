using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	public class ScenarioPhaseDataPlayAnimatin : BaseScenarioPhaseData
	{
		public string targetName;
		public string animationName;
	}

	class ScenarioPhasePlayAnimatin : BaseScenarioPhase
	{
		public string targetName;
		public string animationName;

		public override void Enter()
		{
			INonPlayerCharacter nonPlayer = _gameKernal.GetNonPlayerCharacter(targetName);
			if (nonPlayer != null && string.IsNullOrEmpty(animationName))
			{
				nonPlayer.PlayAnimation(animationName);
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
			ScenarioPhaseDataPlayAnimatin target = (ScenarioPhaseDataPlayAnimatin)data;
			ScenarioPhasePlayAnimatin result = new ScenarioPhasePlayAnimatin();

			result.targetName = target.targetName;
			result.animationName = target.animationName;

			return result;
		}
	}
}