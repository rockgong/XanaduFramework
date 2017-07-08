using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonVector3Literal : BaseCommonVector3
	{
		public float x;
		public float y;
		public float z;
	}

	class CommonVector3LiteralEval : BaseCommonVector3Eval
	{
		public float x;
		public float y;
		public float z;

		public override Vector3 GetVector3()
		{
			return new Vector3(x, y, z);
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Literal target = (CommonVector3Literal)data;
			CommonVector3LiteralEval result = new CommonVector3LiteralEval();

			result.x = target.x;
			result.y = target.y;
			result.z = target.z;

			return result;
		}
	}
}