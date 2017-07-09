using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	class MainGameCameraController
	{
		private ICamera _camera;
		private BaseCommonVector3Eval _cameraPosition;
		private BaseCommonVector3Eval _cameraTarget;
		private MonoDelegate _delegate;

		public BaseCommonVector3Eval cameraPosition
		{
			get {return _cameraPosition;}
			set {_cameraPosition = value;}
		}

		public BaseCommonVector3Eval cameraTarget
		{
			get {return _cameraTarget;}
			set {_cameraTarget = value;}
		}

		public void Initialize(ICamera camera)
		{
			_camera = camera;
		}

		public void Startup()
		{
			_delegate = MonoDelegate.Create(ProcessCamera, "MainGameCameraControllerDelegate");
		}

		private void ProcessCamera()
		{
			if (_camera == null || _cameraPosition == null || _cameraTarget == null)
				return;

			Vector3 target = _cameraTarget.GetVector3();
			Vector3 position = _cameraPosition.GetVector3();

			_camera.EasingMoveTo(target);
			_camera.offset = position - target;

			return;
		}

		public void Shutdown()
		{
			if (_delegate != null)
			{
				GameObject.Destroy(_delegate.gameObject);
				_delegate = null;
			}
		}
	}
}