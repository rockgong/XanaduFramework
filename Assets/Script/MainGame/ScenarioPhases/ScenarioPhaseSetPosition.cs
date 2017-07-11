using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	public class ScenarioPhaseDataSetPosition : BaseScenarioPhaseData
	{
		public string targetName;
		public string standPointName;
		public string lookPointName;
	}

	class ScenarioPhaseSetPosition : BaseScenarioPhase
	{
		public string targetName;
		public string standPointName;
		public string lookPointName;

		public override void Enter()
		{
			INonPlayerCharacter nonPlayer = _gameKernal.GetNonPlayerCharacter(targetName);
			if (nonPlayer != null)
			{
				Transform standTrans = _scene.FindNode(standPointName);
				Vector3 standPoint = standTrans == null ? Vector3.zero : standTrans.position;
				Transform lookTrans = _scene.FindNode(lookPointName);
				Vector3 lookPoint = lookTrans == null ? Vector3.zero : lookTrans.position;
				nonPlayer.position = standPoint;
                Vector3 lookVector = lookPoint - standPoint;
                float yaw = MathHelper.Vector3ToYaw(lookVector);
                nonPlayer.yaw = yaw;
			}

			return;
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
			ScenarioPhaseDataSetPosition target = (ScenarioPhaseDataSetPosition)data;
			ScenarioPhaseSetPosition result = new ScenarioPhaseSetPosition();

			result.targetName = target.targetName;
			result.standPointName = target.standPointName;
			result.lookPointName = target.lookPointName;

			return result;
		}
	}
}