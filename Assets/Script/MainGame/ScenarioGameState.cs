using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	interface IScenarioGameStateHost
	{
		void OnScenarioEnd();
	}

	abstract class BaseScenarioPhase
	{
        protected IGameKernal _gameKernal;
        protected MonoScenarioScene _scene;
        protected InlineUIView _inlineUIView;

		public virtual void Setup(IGameKernal kernal, MonoScenarioScene scene, InlineUIView iuv)
		{
            _gameKernal = kernal;
            _scene = scene;
            _inlineUIView = iuv;
		}

		public abstract void Enter();
		public abstract bool CheckOver();
		public abstract void Exit();
	}

	class ScenarioGameState : IGameState
	{
		private MonoScenarioScene _scene;
		private IGameKernal _gameKernal;
		private IScenarioGameStateHost _host;
        private bool _phaseProcess;
        private BaseScenarioPhase _phase;
        private bool _firstProcess = false;
        private MonoDelegate _delegate = null;

		public void Initialize(IGameKernal kernal, IScenarioGameStateHost host)
		{
			_gameKernal = kernal;
			_host = host;

			return;
		}

		public void Setup(MonoScenarioScene scene, BaseScenarioPhase phase)
		{
			_scene = scene;
			_phase = phase;

			return;
		}

        public void EnterState(IGameKernal kernal)
        {
        	_gameKernal.GetCamera().attachTransform = _scene.cameraRoot;
            _phaseProcess = true;
            _firstProcess = true;
        	_delegate = MonoDelegate.Create(ProcessPhase, "ScenarioGameStateDelegate");
        }

        public void ExitState(IGameKernal kernal)
        {
        	_gameKernal.GetCamera().attachTransform = null;
        	if (_delegate != null)
        	{
        		GameObject.Destroy(_delegate.gameObject);
        		_delegate = null;
        	}
        }

        private void ProcessPhase()
        {
        	if (!_phaseProcess)
        		return;

        	if (_phase != null)
        	{
        		if (_firstProcess)
        		{
        			_firstProcess = false;
        			_phase.Enter();
        		}
        		if (_phase.CheckOver())
        		{
        			_phase.Exit();
                    _phaseProcess = false;
                    if (_host != null)
        				_host.OnScenarioEnd();
        		}
        	}
        }
	}
}