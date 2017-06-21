using System;

namespace GameKernal
{
    class GameKernal : BaseGameKernal
    {
        private Player _player;
        private Stage _stage;
        private GameCamera _camera;

        public override IPlayerCharacter GetPlayerCharacter()
        {
            return _player;
        }

        public override void Initialize(GameKernalDesc desc)
        {
            _camera = new GameCamera();
            _camera.Initialize();

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

        public override ICamera GetCamera()
        {
            return _camera;
        }

        public override void SetCameraFollowPlayer(bool enable)
        {
            if (_camera == null || _player == null)
                return;

            if (enable)
                _camera.SetFollowTransform(_player.GetPlayerTransform());
            else
                _camera.SetFollowTransform(null);

            return;
        }

        public override void Uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}
