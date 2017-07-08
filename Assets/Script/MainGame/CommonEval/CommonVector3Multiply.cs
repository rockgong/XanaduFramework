using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonVector3Multiply : BaseCommonVector3
	{
		public BaseCommonVector3 op0;
		public BaseCommonVector3 op1;
	}

	class CommonVector3MultiplyEval : BaseCommonVector3Eval
	{
		public BaseCommonVector3Eval op0;
		public BaseCommonVector3Eval op1;

		public override Vector3 GetVector3()
		{
			Vector3 op0Val = op0 == null ? Vector3.zero : op0.GetVector3();
			Vector3 op1Val = op1 == null ? Vector3.zero : op1.GetVector3();

			return new Vector3(op0Val.x * op1Val.x, 
							   op0Val.y * op1Val.y,
							   op0Val.z * op1Val.z);
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Multiply target = (CommonVector3Multiply)data;
			CommonVector3MultiplyEval result = new CommonVector3MultiplyEval();

			result.op0 = builder.Build(target.op0, builder);
			result.op1 = builder.Build(target.op1, builder);

			return result;
		}
	}
}