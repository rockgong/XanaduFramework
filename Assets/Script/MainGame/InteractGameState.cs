﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;

namespace MainGame
{
	interface IInteractGameStateHost
	{
		void OnInteractEnd();
	}

    abstract class BaseInteractCommand
    {
        public abstract void Excute(InteractView view);
    }

	class InteractGameState : IGameState, IInteractViewListener
	{
		private IPlayerCharacter _player;
		private INonPlayerCharacter _nonPlayer;

		private IInteractGameStateHost _host;
		private IGameKernal _kernal;
		private ICamera _camera;

		private InteractView _interactView;

        private List<BaseInteractCommand> _commandList;
        private int _commandIndex;

        public void SetCommandList(List<BaseInteractCommand> list)
        {
            _commandList = list;
        }

		public void SetInteractView(InteractView view)
		{
			_interactView = view;

			return;
		}

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
			_kernal = kernal;
			_camera = kernal.GetCamera();
			_kernal.SetCameraFollowPlayer(false);

			_camera.EasingMoveTo(_nonPlayer.viewPosition, () =>
			{
				Vector2 position = UIUtils.WorldPointToCanvasAnchoredPosition(_nonPlayer.position + new Vector3(0.0f, 5.0f, 0.0f), new Vector2(1280.0f, 720.0f));
				Debug.Log(string.Format("Screen Point : {0}", position));
                ProcessCommand();
            });

            _commandIndex = 0;
		}

		public void ExitState(IGameKernal kernal)
		{

		}

		public void OnViewClosed()
		{
            ProcessCommand();
        }

        private void ProcessCommand()
        {
            if (_commandIndex >= _commandList.Count)
                _host.OnInteractEnd();
            else
                _commandList[_commandIndex].Excute(_interactView);
            _commandIndex++;
        }
	}
}