using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public interface IStageDatabaseEntry
    {
        int id { get; }
        string prefabName { get; }
        BaseCommonVector3 cameraLook { get; }
        BaseCommonVector3 cameraPos { get; }
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

    class PlayerStageManager
    {
        private IStageDatabase _database;
        private IGameKernal _gameKernal;

        private int _currentStageId = 0;

        private List<IPlayerStageManagerListener> _listeners = new List<IPlayerStageManagerListener>();

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

        public void SwapPlayer(int stageId, string stagePointName)
        {
            IStage stage = null;
            if (stageId == _currentStageId)
                stage = _gameKernal.GetStage();
            else
            {
                IStageDatabaseEntry entry = _database.GetEntryById(stageId);
                if (entry != null)
                {
                    GameObject proto = Resources.Load<GameObject>(string.Format("Stage/{0}", entry.prefabName));
                    if (proto == null)
                        Debug.LogError(string.Format("Missing stage prefab : {0}", entry.prefabName));
                    stage = _gameKernal.SetupStage(new StageDesc(proto));
                    for (int i = 0; i < _listeners.Count; i++)
                        _listeners[i].OnStageChanged(stageId);
                }
                else
                    Debug.LogError(string.Format("Trying to swap to missing stage : {0}", stageId));
            }

            if (stage != null)
            {
                IPlayerCharacter player = _gameKernal.GetPlayerCharacter();
                player.position = _gameKernal.GetStage().GetStagePoint(stagePointName);
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnPlayerSwapped(stageId, stagePointName);
            }

            _currentStageId = stageId;
        }
    }
}