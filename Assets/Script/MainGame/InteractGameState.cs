using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;
using Helper;

namespace MainGame
{
	interface IInteractGameStateHost
	{
		void OnCommandProcessEnd();
	}

    abstract class BaseInteractCommand
    {
        public abstract void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop);
        public abstract bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop);
    }

	class InteractGameState : IGameState, IInteractViewListener
	{
		private IPlayerCharacter _player;
		private INonPlayerCharacter _nonPlayer;
        private IPropObject _propObject;

		private IInteractGameStateHost _host;
		private IGameKernal _kernal;
		private ICamera _camera;

		private InteractView _interactView;

        private List<BaseInteractCommand> _commandList;
        private int _commandIndex;

        private MonoDelegate _monoDelegate;
        private bool _commandProcessing;

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

        public IPropObject propObject
        {
            get
            {
                return _propObject;
            }
            set
            {
                _propObject = value;
            }
        }

		public void EnterState(IGameKernal kernal)
		{
			_kernal = kernal;
			_camera = kernal.GetCamera();
			_kernal.SetCameraFollowPlayer(false);

            Vector3 theViewPosition = Vector3.zero;
            if (_nonPlayer != null)
                theViewPosition = _nonPlayer.viewPosition;
            else if (_propObject != null)
                theViewPosition = _propObject.position;

			_camera.EasingMoveTo(theViewPosition, () =>
			{
                if (_commandList.Count != 0)
                    _commandList[0].Excute(_interactView, _player, _nonPlayer, _propObject);
                _monoDelegate = MonoDelegate.Create(ProcessCommand, "InteractDelegate");
                _commandProcessing = true;
            });

            _commandIndex = 0;

            FacePlayerToTarget();
		}

        private void FacePlayerToTarget()
        {
            if (_player != null)
            {
                if (_nonPlayer != null)
                    _player.yaw = MathHelper.Vector3ToYaw(_nonPlayer.position - _player.position);
                else if (_propObject != null)
                    _player.yaw = MathHelper.Vector3ToYaw(_propObject.position - _player.position);
            }
        }

		public void ExitState(IGameKernal kernal)
		{
            GameObject.Destroy(_monoDelegate.gameObject);
		}

		public void OnViewClosed()
		{
            //
        }

        private void ProcessCommand()
        {
            /*
            if (_commandIndex >= _commandList.Count)
                _host.OnInteractEnd();
            else
                _commandList[_commandIndex].Excute(_interactView, _player, _nonPlayer, _propObject);
            _commandIndex++;
            */
            if (!_commandProcessing)
                return;
            if (_commandList == null || _commandList.Count == 0)
            {
                _host.OnCommandProcessEnd();
                _commandProcessing = false;
            }
            else
            {
                if (_commandList[_commandIndex].CheckOver(_interactView, _player, _nonPlayer, _propObject))
                {
                    _commandIndex++;
                    if (_commandIndex < _commandList.Count)
                        _commandList[_commandIndex].Excute(_interactView, _player, _nonPlayer, _propObject);
                    else
                    {
                        _host.OnCommandProcessEnd();
                        _commandProcessing = false;
                    }
                }
            }
        }
	}
}