using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class MathHelper
    {
        public static float Vector3ToYaw(Vector3 vec)
        {
            Vector3 finalVec = new Vector3(vec.x, 0.0f, vec.z);
            Vector3 cross = Vector3.Cross(Vector3.forward, finalVec);
            float result = Vector3.Angle(Vector3.forward, finalVec);
            if (cross.y < 0.0f)
                result = 360.0f - result;
            return result;
        }
    }
}