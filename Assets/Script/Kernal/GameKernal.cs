using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameKernal
{
    class GameKernal : BaseGameKernal, IInteractListener
    {
        class NonPlayerEntry
        {
            public string name;
            public Player nonPlayer;
        }

        private Player _player;
        private List<NonPlayerEntry> _nonPlayer = new List<NonPlayerEntry>();
        private Stage _stage;
        private GameCamera _camera;

        private InteractSystem _interactSystem = new InteractSystem();

        private MonoGameKernal _monoGameKernal;

        public override IPlayerCharacter GetPlayerCharacter()
        {
            return _player;
        }

        public override void Initialize(GameKernalDesc desc)
        {
            _camera = new GameCamera();
            _camera.Initialize();

            GameObject go = new GameObject("MonoGameKernal");
            _monoGameKernal = go.AddComponent<MonoGameKernal>();
            _monoGameKernal.updateAction = OnUpdate;
            _interactSystem.AddInteractionListener(this);

            return;
        }

        public override IPlayerCharacter SetupPlayerCharacter(PlayerCharacterDesc desc)
        {
            if (_player != null)
                _player.Uninitialize();

            Player newPlayer = new Player();
            newPlayer.Initialize(desc);
            _player = newPlayer;
            _interactSystem.Initialize(_player);

            return _player;
        }

        public override INonPlayerCharacter AddNonPlayerCharacter(string name, NonPlayerCharacterDesc desc)
        {
            for (int i = 0; i < _nonPlayer.Count; i++)
            {
                if (_nonPlayer[i].name == name)
                    return null;
            }

            NonPlayerEntry entry = new NonPlayerEntry();
            entry.name = name;

            Player newPlayer = new Player();
            newPlayer.Initialize(desc);
            entry.nonPlayer = newPlayer;

            _nonPlayer.Add(entry);
            _interactSystem.AddInteractObject(newPlayer);

            return newPlayer;
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

        public void OnUpdate(float deltaTime)
        {
            _interactSystem.UpdateReadyToInteract();
        }

        public override void Uninitialize()
        {
            throw new NotImplementedException();
        }

        public override void TryInteract()
        {
            _interactSystem.TryInteract();
        }

        public void OnInteractHappen(IInteractSubject sub, IInteractObject obj)
        {
            Debug.Log("Action !");
        }
    }
}
