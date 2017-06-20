using System;

namespace GameKernal
{
    class GameKernal : BaseGameKernal
    {
        private Player _player;
        private Stage _stage;

        public override IPlayerCharacter GetPlayerCharaccter()
        {
            return _player;
        }

        public override void Initialize(GameKernalDesc desc)
        {
            return;
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

        public override IStage SetupStage(StageDesc desc)
        {
            if (_stage != null)
                _stage.Uninitialize();

            Stage newStage = new Stage();
            newStage.Initialize(desc);
            _stage = newStage;

            return _stage;
        }

        public override void Uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}
