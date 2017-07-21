using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	class MainGameState : IGameState, IInputEventListener
	{
		private MonoGameInput _input;
		private IPlayerCharacter _player;
		private ICamera _camera;
		private bool _mouseDown;
		private IGameKernal _kernal;
		private MainGameCameraController _camController;

		public void SetCameraController(MainGameCameraController cc)
		{
			_camController = cc;
		}

		public void EnterState(IGameKernal kernal)
		{
			if (_input == null)
			{
				GameObject go = new GameObject("Input");
				_input = go.AddComponent<MonoGameInput>();
			}
			_input.listener = this;
			_player = kernal.GetPlayerCharacter();
			_camera = kernal.GetCamera();
			// kernal.SetCameraFollowPlayer(true);
			_camController.Startup();
			_kernal = kernal;

			return;
		}

		public void ExitState(IGameKernal kernal)
		{
			_input.listener = null;
			// kernal.SetCameraFollowPlayer(false);
			_camController.Shutdown();
            _player.velocity = 0.0f;

			return;
		}

		public void OnMouseMove(Vector3 newPosition)
		{
			if (_mouseDown)
				SetPlayerYawFromMousePosition();
		}

		public void OnMouseButtonDown()
		{
			SetPlayerYawFromMousePosition();
			_player.velocity = 5.0f;
			_mouseDown = true;
		}

		public void OnMouseButtonUp()
		{
			_player.velocity = 0.0f;
			_mouseDown = false;
		}

		public void OnMouseButtonRightDown()
		{
			_kernal.TryInteract();
		}

		private void SetPlayerYawFromMousePosition()
		{
			Ray cameraRay = Camera.main.ScreenPointToRay(_input.GetMousePosition());
			Plane groundPlane = new Plane(Vector3.up, _player.controlPosition);

			float enter;
			if (groundPlane.Raycast(cameraRay, out enter))
			{
				Vector3 point = cameraRay.GetPoint(enter);
				Vector3 dir = point - _player.position;
				float angle = Mathf.Atan(dir.x / dir.z) / Mathf.PI * 180.0f;
				if (dir.z < 0.0f)
					angle += 180.0f;
				_player.yaw = angle;
			}
		}
	}
}