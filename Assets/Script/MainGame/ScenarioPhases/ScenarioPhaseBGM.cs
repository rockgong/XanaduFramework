using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;
using Audio;

namespace MainGame
{
	public class ScenarioPhaseDataBGM : BaseScenarioPhaseData
	{
		public string bgmName = string.Empty;
	}

	class ScenarioPhaseBGM : BaseScenarioPhase
	{
		public string bgmName = string.Empty;

		public override void Enter()
		{
			BGM.Play(bgmName);
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
			ScenarioPhaseDataBGM target = (ScenarioPhaseDataBGM)data;
			ScenarioPhaseBGM result = new ScenarioPhaseBGM();

			result.bgmName = target.bgmName;

			return result;
		}
	}
}