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

		public override Vector3 easingTarget
		{
			get
			{
				if (_monoCamera != null)
					return _monoCamera.easingTargetPosition;
				return Vector3.zero;
			}
		}

		public override Transform attachTransform
		{
			get
			{
                if (_monoCamera != null)
                    return _monoCamera.attachTransform;
                return null;
			}
			set
			{
                _monoCamera.attachTransform = value;
			}
		}

		public void SetFollowTransform(Transform target)
		{
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

        public override void EasingMoveTo(Vector3 target, System.Action onFinish = null)
        {
        	_monoCamera.easingMoving = true;
        	_monoCamera.easingTargetPosition = target;
        	_monoCamera.onFinish = onFinish;

        	return;
        }
	}
}