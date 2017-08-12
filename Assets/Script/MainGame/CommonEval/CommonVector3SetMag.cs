using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3SetMag : BaseCommonVector3
	{
		public BaseCommonVector3 op;
		public float mag;
	}

	class CommonVector3SetMagEval : BaseCommonVector3Eval
	{
		public BaseCommonVector3Eval op;
		public float mag;

		public override void Setup(IGameKernal gameKernal)
		{
			base.Setup(gameKernal);

			if (op != null)
				op.Setup(gameKernal);
		}

		public override Vector3 GetVector3()
		{
			Vector3 targetVal = op.GetVector3();
			
			return targetVal == Vector3.zero ? Vector3.zero : targetVal.normalized * mag;
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3SetMag target = (CommonVector3SetMag)data;
			CommonVector3SetMagEval result = new CommonVector3SetMagEval();

			result.op = builder.Build(target.op);
			result.mag = target.mag;

			return result;
		}
	}
}