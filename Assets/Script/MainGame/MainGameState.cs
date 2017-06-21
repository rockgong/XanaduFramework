using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class MainGameState : IGameState, IInputEventListener
	{
		private MonoGameInput _input;
		private IPlayerCharacter _player;
		private ICamera _camera;
		private bool _mouseDown;

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
			_camera.offset = new Vector3(0.0f, 10.0f, -10.0f);
			kernal.SetCameraFollowPlayer(true);


			return;
		}

		public void ExitState(IGameKernal kernal)
		{
			_input.listener = null;
			kernal.SetCameraFollowPlayer(false);

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
			Debug.Log(string.Format("Mouse Up : {0}", _input.GetMousePosition()));
			_player.velocity = 0.0f;
			_mouseDown = false;
		}

		private void SetPlayerYawFromMousePosition()
		{
			Ray cameraRay = Camera.main.ScreenPointToRay(_input.GetMousePosition());
			Plane groundPlane = new Plane(Vector3.up, _player.position.y);
			Debug.Log(string.Format("Plane : {0},{1}", groundPlane.distance, groundPlane.normal));
			float enter;
			if (groundPlane.Raycast(cameraRay, out enter))
			{
				Vector3 point = cameraRay.GetPoint(enter);
				Vector3 dir = point - _player.position;
				float angle = Mathf.Atan(dir.x / dir.z) / Mathf.PI * 180.0f;
				if (dir.z < 0.0f)
					angle += 180.0f;
				Debug.Log(string.Format("Angle : {0}", angle));
				_player.yaw = angle;
			}
		}
	}
}