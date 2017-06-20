using System;

namespace GameKernal
{
    abstract class BaseGameKernal : IGameKernal
    {
        // Interface implement
        public abstract INonPlayerCharacter AddNonPlayerCharacter(string name, NonPlayerCharacterDesc desc);

        public abstract IPropObject AddPropObject(string name, PropObjectDesc desc);

        public abstract INonPlayerCharacter GetNonPlayerCharacter(string name);

        public abstract IPlayerCharacter GetPlayerCharaccter();

        public abstract IPropObject GetPropObject(string name);

        public abstract void RemoveNonPlayerCharacter(string name);

        public abstract void RemoveNonPlayerCharacter(INonPlayerCharacter handler);

        public abstract void RemovePropObject(string name);

        public abstract void RemovePropObject(IPropObject desc);

        public abstract IPlayerCharacter SetupPlayerCharacter(PlayerCharacterDesc desc);

        public abstract GameKernalErrorCode Shutdown();

        public abstract GameKernalErrorCode Startup();

        public abstract IStage SetupStage(StageDesc desc);

        public abstract IStage GetStage();

        // Class public method
        public abstract void Initialize(GameKernalDesc desc);

        public abstract void Uninitialize();
    }
}