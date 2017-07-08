using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3StagePoint : BaseCommonVector3
	{
		public string pointName;
	}

	class CommonVector3StagePointEval : BaseCommonVector3Eval
	{
		public string pointName;

		public override Vector3 GetVector3()
		{
			IStage stage = _gameKernal.GetStage();
			if (stage != null)
				return stage.GetStagePoint(pointName);

			return Vector3.zero;
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3StagePoint target = (CommonVector3StagePoint)data;
			CommonVector3StagePointEval result = new CommonVector3StagePointEval();

			result.pointName = target.pointName;

			return result;
		}
	}
}