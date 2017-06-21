using System;

namespace GameKernal
{
    abstract class BaseGameKernal : IGameKernal
    {
        protected IGameState _currentState;

        // Interface implement
        public virtual INonPlayerCharacter AddNonPlayerCharacter(string name, NonPlayerCharacterDesc desc)
        {
            return null;
        }

        public virtual IPropObject AddPropObject(string name, PropObjectDesc desc)
        {
            return null;
        }

        public virtual INonPlayerCharacter GetNonPlayerCharacter(string name)
        {
            return null;
        }

        public virtual IPlayerCharacter GetPlayerCharacter()
        {
            return null;
        }

        public virtual IPropObject GetPropObject(string name)
        {
            return null;
        }

        public virtual void RemoveNonPlayerCharacter(string name)
        {
            return;
        }

        public virtual void RemoveNonPlayerCharacter(INonPlayerCharacter handler)
        {
            return;
        }

        public virtual void RemovePropObject(string name)
        {
            return;
        }

        public virtual void RemovePropObject(IPropObject desc)
        {
            return;
        }

        public virtual IPlayerCharacter SetupPlayerCharacter(PlayerCharacterDesc desc)
        {
            return null;
        }

        public virtual GameKernalErrorCode Shutdown()
        {
            return GameKernalErrorCode.OK;
        }

        public virtual GameKernalErrorCode Startup()
        {
            return GameKernalErrorCode.OK;
        }

        public virtual IStage SetupStage(StageDesc desc)
        {
            return null;
        }

        public virtual IStage GetStage()
        {
            return null;
        }

        public virtual ICamera GetCamera()
        {
            return null;
        }

        public virtual void SetCameraFollowPlayer(bool enable)
        {
            return;
        }

        public virtual void SetGameState(IGameState state)
        {
            if (_currentState == state)
                return;

            if (_currentState != null)
                _currentState.ExitState(this);

            _currentState = state;
            _currentState.EnterState(this);

            return;
        }

        // Class public method
        public abstract void Initialize(GameKernalDesc desc);

        public abstract void Uninitialize();
    }
}