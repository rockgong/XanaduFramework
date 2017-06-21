using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	public class MonoCamera : MonoBehaviour {
		public Transform lookAtTransform;
		public Vector3 lookAtPosition;
		public Vector3 offset;

		private void LateUpdate()
		{
			if (lookAtTransform == null)
				transform.position = lookAtPosition + offset;
			else
			{
				transform.position = lookAtTransform.position + offset;
				lookAtPosition = lookAtTransform.position;
			}
			transform.LookAt(lookAtPosition);
		}
	}
}