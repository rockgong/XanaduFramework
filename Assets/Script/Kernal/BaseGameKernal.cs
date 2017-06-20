using System;

namespace GameKernal
{
    abstract class BaseGameKernal : IGameKernal
    {
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

        public virtual IPlayerCharacter GetPlayerCharaccter()
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

        // Class public method
        public abstract void Initialize(GameKernalDesc desc);

        public abstract void Uninitialize();
    }
}