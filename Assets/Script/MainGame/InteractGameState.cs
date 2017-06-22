using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	interface IInteractGameStateHost
	{
		void OnInteractEnd();
	}

	class InteractGameState : IGameState
	{
		private IPlayerCharacter _player;
		private INonPlayerCharacter _nonPlayer;

		private IInteractGameStateHost _host;
		private IGameKernal _kernal;
		private ICamera _camera;

		public void SetHost(IInteractGameStateHost host)
		{
			_host = host;

			return;
		}

		public IPlayerCharacter player
		{
			get
			{
				return _player;
			}
			set
			{
				_player = value;
			}
		}

		public INonPlayerCharacter nonPlayer
		{
			get
			{
				return _nonPlayer;
			}
			set
			{
				_nonPlayer = value;
			}
		}

		public void EnterState(IGameKernal kernal)
		{
			GameObject go = new GameObject("Timer");
			MonoTimer timer = go.AddComponent<MonoTimer>();
			timer.Setup(1.0f, () =>
			{
				if (_host != null)
					_host.OnInteractEnd();
			});
			_kernal = kernal;
			_camera = kernal.GetCamera();
			_kernal.SetCameraFollowPlayer(false);
			_camera.lookPosition = _nonPlayer.position;
		}

		public void ExitState(IGameKernal kernal)
		{

		}
	}
}