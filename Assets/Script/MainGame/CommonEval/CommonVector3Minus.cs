using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3Minus : BaseCommonVector3
	{
		public BaseCommonVector3 op0;
		public BaseCommonVector3 op1;
	}

	class CommonVector3MinusEval : BaseCommonVector3Eval
	{
		public BaseCommonVector3Eval op0;
		public BaseCommonVector3Eval op1;

		public override void Setup(IGameKernal gameKernal)
		{
			base.Setup(gameKernal);

			if (op0 != null)
				op0.Setup(gameKernal);
			if (op1 != null)
				op1.Setup(gameKernal);
		}

		public override Vector3 GetVector3()
		{
			Vector3 op0Val = op0 == null ? Vector3.zero : op0.GetVector3();
			Vector3 op1Val = op1 == null ? Vector3.zero : op1.GetVector3();

			return op0Val - op1Val;
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Minus target = (CommonVector3Minus)data;
			CommonVector3MinusEval result = new CommonVector3MinusEval();

			result.op0 = builder.Build(target.op0, builder);
			result.op1 = builder.Build(target.op1, builder);

			return result;
		}
	}
}