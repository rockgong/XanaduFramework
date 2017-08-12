using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3Lerp : BaseCommonVector3
	{
		public BaseCommonVector3 lerpPoint0;
		public BaseCommonVector3 lerpPoint1;
		public float lerpValue;
	}

	class CommonVector3LerpEval : BaseCommonVector3Eval
	{
		public BaseCommonVector3Eval lerpPoint0;
		public BaseCommonVector3Eval lerpPoint1;
		public float lerpValue;

		public override void Setup(IGameKernal gameKernal)
		{
			base.Setup(gameKernal);

			if (lerpPoint0 != null)
				lerpPoint0.Setup(gameKernal);
			if (lerpPoint1 != null)
				lerpPoint1.Setup(gameKernal);
		}
		
		public override Vector3 GetVector3()
		{
			Vector3 p0 = lerpPoint0.GetVector3();
			Vector3 p1 = lerpPoint1.GetVector3();

			return p1 * lerpValue + p0 * (1 - lerpValue);
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Lerp target = (CommonVector3Lerp)data;
			CommonVector3LerpEval result = new CommonVector3LerpEval();

			result.lerpPoint0 = builder.Build(target.lerpPoint0);
			result.lerpPoint1 = builder.Build(target.lerpPoint1);
			result.lerpValue = target.lerpValue;

			return result;
		}
	}
}