using System;

namespace GameKernal
{
    class GameKernal : BaseGameKernal
    {
        private Player _player;

        public override INonPlayerCharacter AddNonPlayerCharacter(string name, NonPlayerCharacterDesc desc)
        {
            throw new NotImplementedException();
        }

        public override IPropObject AddPropObject(string name, PropObjectDesc desc)
        {
            throw new NotImplementedException();
        }

        public override INonPlayerCharacter GetNonPlayerCharacter(string name)
        {
            throw new NotImplementedException();
        }

        public override IPlayerCharacter GetPlayerCharaccter()
        {
            return _player;
        }

        public override IPropObject GetPropObject(string name)
        {
            throw new NotImplementedException();
        }

        public override void Initialize(GameKernalDesc desc)
        {
            return;
        }

        public override void RemoveNonPlayerCharacter(string name)
        {
            throw new NotImplementedException();
        }

        public override void RemoveNonPlayerCharacter(INonPlayerCharacter handler)
        {
            throw new NotImplementedException();
        }

        public override void RemovePropObject(string name)
        {
            throw new NotImplementedException();
        }

        public override void RemovePropObject(IPropObject desc)
        {
            throw new NotImplementedException();
        }

        public override IPlayerCharacter SetupPlayerCharacter(PlayerCharacterDesc desc)
        {
            if (_player != null)
                _player.Uninitialize();

            Player newPlayer = new Player();
            newPlayer.Initialize(desc);
            _player = newPlayer;

            return _player;
        }

        public override GameKernalErrorCode Shutdown()
        {
            throw new NotImplementedException();
        }

        public override GameKernalErrorCode Startup()
        {
            return GameKernalErrorCode.OK;
        }

        public override void Uninitialize()
        {
            throw new NotImplementedException();
        }

        public override IStage SetupStage(StageDesc desc)
        {
            return null;
        }

        public override IStage GetStage()
        {
            return null;
        }
    }
}
