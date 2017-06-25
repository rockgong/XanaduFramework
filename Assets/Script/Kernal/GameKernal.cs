using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameKernal
{
    class GameKernal : BaseGameKernal, IInteractListener
    {
        private Player _player;
        private List<Player> _nonPlayer = new List<Player>();
        private List<PropObject> _propObject = new List<PropObject>();
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

            Player newPlayer = new Player();
            newPlayer.name = name;
            newPlayer.Initialize(desc);

            _nonPlayer.Add(newPlayer);
            _interactSystem.AddInteractObject(newPlayer);

            return newPlayer;
        }

        public override IPropObject AddPropObject(string name, PropObjectDesc desc)
        {
            for (int i = 0; i < _nonPlayer.Count; i++)
            {
                if (_nonPlayer[i].name == name)
                    return null;
            }

            PropObject newProp = new PropObject();
            newProp.name = name;
            newProp.Initialize(desc);

            _propObject.Add(newProp);
            _interactSystem.AddInteractObject(newProp);

            return newProp;
        }

        public override IPropObject GetPropObject(string name)
        {
            return base.GetPropObject(name);
        }

        public override void RemovePropObject(string name)
        {
            base.RemovePropObject(name);
        }

        public override void RemovePropObject(IPropObject desc)
        {
            base.RemovePropObject(desc);
        }

        public override void ClearNonPlayer()
        {
            for (int i = 0; i < _nonPlayer.Count; i++)
            {
                _nonPlayer[i].Uninitialize();
                _interactSystem.RemoveInteractObject(_nonPlayer[i]);
            }
            _nonPlayer.Clear();
        }

        public override void ClearPropObject()
        {
            for (int i = 0; i < _propObject.Count; i++)
            {
                _propObject[i].Uninitialize();
                _interactSystem.RemoveInteractObject(_propObject[i]);
            }
            _propObject.Clear();
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

        public override IStage GetStage()
        {
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
                _camera.SetFollowTransform(_player.GetViewTransform());
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
            if (host != null)
            {
                if (sub is IPlayerCharacter)
                {
                    IPlayerCharacter pc = (IPlayerCharacter)sub;
                    if (obj is INonPlayerCharacter)
                    {
                        host.OnInteract(pc, (INonPlayerCharacter)obj);
                    }
                    else if (obj is IPropObject)
                    {
                        host.OnInteract(pc, (IPropObject)obj);
                    }
                }
            }
        }
    }
}
