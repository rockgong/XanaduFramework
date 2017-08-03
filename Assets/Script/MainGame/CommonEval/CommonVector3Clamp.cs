using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3Clamp : BaseCommonVector3
	{
		public BaseCommonVector3 targetPoint;
		public BaseCommonVector3 clampPoint0;
		public BaseCommonVector3 clampPoint1;
	}

	class CommonVector3ClampEval : BaseCommonVector3Eval
	{
		public BaseCommonVector3Eval targetPoint;
		public BaseCommonVector3Eval clampPoint0;
		public BaseCommonVector3Eval clampPoint1;

		public override void Setup(IGameKernal gameKernal)
		{
			base.Setup(gameKernal);

			if (targetPoint != null)
				targetPoint.Setup(gameKernal);
			if (clampPoint0 != null)
				clampPoint0.Setup(gameKernal);
			if (clampPoint1 != null)
				clampPoint1.Setup(gameKernal);
		}
		
		public override Vector3 GetVector3()
		{
			Vector3 targetPointVector = targetPoint.GetVector3();
			Vector3 p0 = clampPoint0.GetVector3();
			Vector3 p1 = clampPoint1.GetVector3();

			float minX = Mathf.Min(p0.x, p1.x);
			float minY = Mathf.Min(p0.y, p1.y);
			float minZ = Mathf.Min(p0.z, p1.z);

			float maxX = Mathf.Max(p0.x, p1.x);
			float maxY = Mathf.Max(p0.y, p1.y);
			float maxZ = Mathf.Max(p0.z, p1.z);

			float x = Mathf.Clamp(targetPointVector.x, minX, maxX);
			float y = Mathf.Clamp(targetPointVector.y, minY, maxY);
			float z = Mathf.Clamp(targetPointVector.z, minZ, maxZ);

			return new Vector3(x, y, z);
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Clamp target = (CommonVector3Clamp)data;
			CommonVector3ClampEval result = new CommonVector3ClampEval();

			result.targetPoint = builder.Build(target.targetPoint, builder);
			result.clampPoint0 = builder.Build(target.clampPoint0, builder);
			result.clampPoint1 = builder.Build(target.clampPoint1, builder);

			return result;
		}
	}
}