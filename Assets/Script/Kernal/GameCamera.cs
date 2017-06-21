using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	class GameCamera : BaseGameCamera
	{
		private Camera _mainCamera;
		private MonoCamera _monoCamera;

		public override Vector3 lookPosition
		{
			get
			{
				if (_monoCamera != null)
					return _monoCamera.lookAtPosition;
				return Vector3.zero;
			}
			set
			{
				if (_monoCamera != null)
					_monoCamera.lookAtPosition = value;
			}
		}

		public override Vector3 offset
		{
			get
			{
				if (_monoCamera != null)
					return _monoCamera.offset;
				return Vector3.zero;
			}
			set
			{
				if (_monoCamera != null)
					_monoCamera.offset = value;
			}
		}

		public void SetFollowTransform(Transform target)
		{
			if (_monoCamera != null)
				_monoCamera.lookAtTransform = target;
		}

        public override void Initialize()
        {
        	_mainCamera = Camera.main;
        	_monoCamera = _mainCamera.gameObject.AddComponent<MonoCamera>();

        	return;
        }
        public override void Uninitialize()
        {
        	_mainCamera = null;

        	return;
        }
	}
}