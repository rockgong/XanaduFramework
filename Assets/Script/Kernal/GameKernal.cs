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
        private List<Trigger> _trigger = new List<Trigger>();
        private Stage _stage;
        private GameCamera _camera;

        private InteractSystem _interactSystem = new InteractSystem();

        private MonoGameKernal _monoGameKernal;

        private PlayerCharacterDesc _playerCharacterDesc;
        private StageDesc _stageDesc;

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

            _playerCharacterDesc = desc;

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

        public override INonPlayerCharacter GetNonPlayerCharacter(string name)
        {
            for (int i = 0; i < _nonPlayer.Count; i++)
            {
                if (_nonPlayer[i].name == name)
                    return _nonPlayer[i];
            }
            return null;
        }

        public override void RemoveNonPlayerCharacter(string name)
        {
            for (int i = 0; i < _nonPlayer.Count; i++)
            {
                if (_nonPlayer[i].name == name)
                {
                    _nonPlayer[i].Uninitialize();
                    _interactSystem.RemoveInteractObject(_nonPlayer[i]);
                    _nonPlayer.RemoveAt(i);
                    return;
                }
            }
        }

        public override void RemoveNonPlayerCharacter(INonPlayerCharacter handler)
        {
            if (handler is Player && _nonPlayer.Contains((Player)handler))
            {
                ((Player)handler).Uninitialize();
                _interactSystem.RemoveInteractObject((Player)handler);
                _nonPlayer.Remove((Player)handler);
            }
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
            for (int i = 0; i < _propObject.Count; i++)
            {
                if (_propObject[i].name == name)
                    return _propObject[i];
            }

            return null;
        }

        public override void RemovePropObject(string name)
        {
            for (int i = 0; i < _propObject.Count; i++)
            {
                if (_propObject[i].name == name)
                {
                    _propObject[i].Uninitialize();
                    _interactSystem.RemoveInteractObject(_propObject[i]);
                    _propObject.RemoveAt(i);
                    return;
                }
            }
        }

        public override void RemovePropObject(IPropObject handler)
        {
            if (handler is PropObject && _propObject.Contains((PropObject)handler))
            {
                ((PropObject)handler).Uninitialize();
                _interactSystem.RemoveInteractObject((PropObject)handler);
                _propObject.Remove((PropObject)handler);
            }
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

        public override ITrigger AddTrigger(string name, TriggerDesc desc)
        {
            for (int i = 0; i < _trigger.Count; i++)
            {
                if (_trigger[i].name == name)
                    return null;
            }

            Trigger newTrigger = new Trigger();
            newTrigger.name = name;
            newTrigger.Initialize(desc);

            _trigger.Add(newTrigger);

            return newTrigger;
        }

        public override void RemoveTrigger(string name)
        {
            for (int i = 0; i < _trigger.Count; i++)
            {
                if (_trigger[i].name == name)
                {
                    _trigger[i].Uninitialize();
                    _trigger.RemoveAt(i);
                    return;
                }
            }
        }

        public override void RemoveTrigger(ITrigger handler)
        {
            if (handler is Trigger && _trigger.Contains((Trigger)handler))
            {
                ((Trigger)handler).Uninitialize();
                _trigger.Remove((Trigger)handler);
            }
        }

        public override void ClearTrigger()
        {
            for (int i = 0; i < _trigger.Count; i++)
                _trigger[i].Uninitialize();
            _trigger.Clear();
        }

        public override IStage SetupStage(StageDesc desc, System.Action onEnd = null)
        {
            if (_stage != null)
                _stage.Uninitialize();

            Stage newStage = new Stage();
            newStage.Initialize(desc, onEnd);
            _stage = newStage;

            _stageDesc = desc;

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

        public void OnReadyToInteractObjectChanged(IInteractObject from, IInteractObject to)
        {
            if (host != null)
            {
                if (from == null || from is INonPlayerCharacter)
                {
                    if (to == null || to is INonPlayerCharacter)
                        host.OnReadyToInteractChanged((INonPlayerCharacter)from, (INonPlayerCharacter)to);
                    else if (to is IPropObject)
                        host.OnReadyToInteractChanged((INonPlayerCharacter)from, (IPropObject)to);
                }
                else if (from is IPropObject)
                {
                    if (to == null || to is INonPlayerCharacter)
                        host.OnReadyToInteractChanged((IPropObject)from, (INonPlayerCharacter)to);
                    else if (to is IPropObject)
                        host.OnReadyToInteractChanged((IPropObject)from, (IPropObject)to);
                }
            }
        }

        public override GameKernalErrorCode Shutdown()
        {
            if (_player != null)
            {
                _player.Uninitialize();
                _player = null;
            }
            if (_stage != null)
            {
                _stage.Uninitialize();
                _stage = null;
            }

            for (int i = 0; i < _nonPlayer.Count; i++)
                _nonPlayer[i].Uninitialize();
            _nonPlayer.Clear();

            for (int i = 0; i < _propObject.Count; i++)
                _propObject[i].Uninitialize();
            _propObject.Clear();

            if (_camera != null)
                _camera.StopGrab();

            _interactSystem.ClearInteractObject();

            return GameKernalErrorCode.OK;
        }

        public override GameKernalErrorCode Startup()
        {
            if (_camera != null)
                _camera.StartGrab();

            return GameKernalErrorCode.OK;
        }
    }
}
