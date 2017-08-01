using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	public class MonoCamera : MonoBehaviour {
        public bool grab = true;
		public Transform lookAtTransform;
		public Vector3 lookAtPosition;
		public Vector3 offset;
		public float maxVelocity = 100.0f;
		public Vector3 easingTargetPosition;
		public float easingMoveFactor = 0.1f;
		public bool easingMoving = false;
        public float easingMoveThreshold = 0.01f;
		public System.Action onFinish = null;
        public Transform attachTransform = null;

		private void LateUpdate()
		{
            if (!grab)
                return;

            if (attachTransform == null)
            {
                if (lookAtTransform == null)
                {
                    if (easingMoving)
                    {
                        lookAtPosition = GetEasingLookAtPosition(lookAtPosition, easingTargetPosition);
                        if (
                            (lookAtPosition - easingTargetPosition).magnitude < 0.01f
                            )
                        {
                            easingMoving = false;
                            if (onFinish != null)
                            {
                                onFinish();
                                onFinish = null;
                            }
                            lookAtPosition = easingTargetPosition;
                        }
                    }
                    transform.position = lookAtPosition + offset;
                }
                else
                {
                    lookAtPosition = GetEasingLookAtPosition(lookAtPosition, lookAtTransform.position);
                    transform.position = lookAtPosition + offset;
                }
                transform.LookAt(lookAtPosition);
            }
            else
            {
                transform.position = attachTransform.position;
                transform.rotation = attachTransform.rotation;
            }
		}

		private Vector3 GetEasingLookAtPosition(Vector3 current, Vector3 target)
		{
			Vector3 distance = target - lookAtPosition;
			float targetDistanceFactor = Mathf.Min(1.0f, easingMoveFactor);
			Vector3 targetDistance = distance * targetDistanceFactor;
			float maxDistanceMag = maxVelocity * Time.deltaTime;
			targetDistance = targetDistance.normalized * Mathf.Min(maxDistanceMag, targetDistance.magnitude);
			return lookAtPosition + targetDistance;
		}
	}
}