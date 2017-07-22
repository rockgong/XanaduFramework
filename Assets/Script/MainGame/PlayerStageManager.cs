using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using System;
using Helper;

namespace MainGame
{
    public class StageTransferData
    {
        public string triggerPointName;
        public int stageId;
        public string stagePointName;
        public string stageLookPointName;
    }

    public interface IStageDatabaseEntry
    {
        int id { get; }
        string sceneName { get; }
        BaseCommonVector3 cameraLook { get; }
        BaseCommonVector3 cameraPos { get; }
        StageTransferData[] transfers {get; }
    }

    public interface IStageDatabase
    {
        IStageDatabaseEntry GetEntryById(int id);
    }

    public interface IPlayerStageManagerListener
    {
        void OnPlayerSwapped(int stageId, string stagePointName);
        void OnStageChanged(int stageId);
    }

    class StageAnimationEntry
    {
        public int stageId;
        public string animationTargetName;
        public string animationStateName;
    }

    class PlayerStageManager
    {
        private IStageDatabase _database;
        private IGameKernal _gameKernal;

        private int _currentStageId = 0;

        private List<IPlayerStageManagerListener> _listeners = new List<IPlayerStageManagerListener>();
        private List<StageAnimationEntry> _stageAnimations = new List<StageAnimationEntry>();

        public void SetDatabase(IStageDatabase db)
        {
            _database = db;
        }

        public void SetGameKernal(IGameKernal kernal)
        {
            _gameKernal = kernal;
        }

        public void RegisterListener(IPlayerStageManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    return;
            }

            _listeners.Add(listener);
        }

        public void UnregisterListener(IPlayerStageManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    _listeners.Remove(listener);
            }
        }

        public void ClearListener()
        {
            _listeners.Clear();
        }

        public void SwapPlayer(int stageId, string stagePointName, string stageLookPointName = null)
        {
            IStage stage = null;
            bool thisFrame = true;
            if (stageId == _currentStageId)
                stage = _gameKernal.GetStage();
            else
            {
                thisFrame = false;
                IStageDatabaseEntry entry = _database.GetEntryById(stageId);
                if (entry != null)
                {
                    stage = _gameKernal.SetupStage(new StageDesc(entry.sceneName), () =>
                    {
                        for (int i = 0; i < _listeners.Count; i++)
                            _listeners[i].OnStageChanged(stageId);
                        IPlayerCharacter player = _gameKernal.GetPlayerCharacter();
                        IStage newStage = _gameKernal.GetStage();
                        player.position = newStage.GetStagePoint(stagePointName);

                        if (string.IsNullOrEmpty(stageLookPointName))
                            player.yaw = 0.0f;
                        else
                        {
                            Vector3 lookPoint = newStage.GetStagePoint(stageLookPointName);
                            Vector3 offset = lookPoint - player.position;
                            player.yaw = MathHelper.Vector3ToYaw(offset);
                        }

                        for (int i = 0; i < _stageAnimations.Count; i++)
                        {
                            if (_stageAnimations[i].stageId == stageId)
                            {
                                newStage.PlayerStageAnimation(_stageAnimations[i].animationTargetName, _stageAnimations[i].animationStateName);
                            }
                        }
                        for (int i = 0; i < _listeners.Count; i++)
                        _listeners[i].OnPlayerSwapped(stageId, stagePointName);
                        _currentStageId = stageId;
                    });
                }
                else
                    Debug.LogError(string.Format("Trying to swap to missing stage : {0}", stageId));
            }

            if (thisFrame && stage != null)
            {
                IPlayerCharacter player = _gameKernal.GetPlayerCharacter();
                player.position = _gameKernal.GetStage().GetStagePoint(stagePointName);
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnPlayerSwapped(stageId, stagePointName);

                _currentStageId = stageId;
            }
        }

        public void SetStagePointAnimation(int stageId, string animationTargetName, string animationStateName)
        {
            StageAnimationEntry entry = null;
            for (int i = 0; i < _stageAnimations.Count; i++)
            {
                if (_stageAnimations[i].stageId == stageId && _stageAnimations[i].animationTargetName == animationTargetName)
                {
                    entry = _stageAnimations[i];
                    break;
                }
            }

            if (entry == null)
            {
                entry = new StageAnimationEntry();
                entry.stageId = stageId;
                entry.animationTargetName = animationTargetName;
                _stageAnimations.Add(entry);
            }

            if (entry != null)
                entry.animationStateName = animationStateName;
        }
    }
}