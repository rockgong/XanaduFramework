using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	public class ScenarioPhaseDataCameraAnimation : BaseScenarioPhaseData
	{
		public string animationName;
	}

	class ScenarioPhaseCameraAnimation : BaseScenarioPhase
	{
		public string animationName;

		public override void Enter()
		{
			Animator anim = _scene.GetComponent<Animator>();
			if (anim != null)
				anim.Play(animationName);
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
			ScenarioPhaseDataCameraAnimation target = (ScenarioPhaseDataCameraAnimation)data;
			ScenarioPhaseCameraAnimation result = new ScenarioPhaseCameraAnimation();

			result.animationName = target.animationName;

			return result;
		}
	}
}