using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public interface INonPlayerDatabaseEntry
    {
        int id { get; }
        string name { get; }
        string prefabName { get; }
    }

    public interface INonPlayerDatabase
    {
        INonPlayerDatabaseEntry GetEntryById(int id);
        List<INonPlayerDatabaseEntry> GetEntryList();
    }

    class NonPlayerInfo
    {
        public INonPlayerDatabaseEntry data;
        public int stageId;
        public string stagePointName;
    }

    interface INonPlayerManagerListener
    {
        void OnNonPlayerPositionChanged(NonPlayerInfo info);
    }

    class NonPlayerManager
    {
        private List<NonPlayerInfo> _nonPlayerInfoList = new List<NonPlayerInfo>();
        private List<INonPlayerManagerListener> _listeners = new List<INonPlayerManagerListener>();
        private IGameKernal _gameKernal;

        public void Initialize(INonPlayerDatabase db, IGameKernal gameKernal)
        {
            List<INonPlayerDatabaseEntry> dataList = db.GetEntryList();
            _nonPlayerInfoList.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                NonPlayerInfo info = new NonPlayerInfo();
                info.data = dataList[i];
                info.stageId = 0;
                info.stagePointName = string.Empty;
                _nonPlayerInfoList.Add(info);
            }
            _gameKernal = gameKernal;
        }

        public void RegisterListener(INonPlayerManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    return;
            }

            _listeners.Add(listener);
        }

        public void UnregisterListener(INonPlayerManagerListener listener)
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

        public void SetNonPlayerPosition(int id, int stageId, string stagePointName)
        {
            NonPlayerInfo info = GetNonPlayerInfo(id);
            if (info != null)
            {
                if (info.stageId == stageId && info.stagePointName == stagePointName)
                    return;
                info.stageId = stageId;
                info.stagePointName = stagePointName;
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnNonPlayerPositionChanged(info);
            }
        }

        public void SetupAllNonPlayers(int stageId)
        {
            _gameKernal.ClearNonPlayer();
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].stageId == stageId)
                {
                    GameObject proto = Resources.Load<GameObject>(string.Format("NonPlayer/{0}", _nonPlayerInfoList[i].data.prefabName));
                    if (proto == null)
                        Debug.LogError(string.Format("Missing NonPlayer player : {0}", _nonPlayerInfoList[i].data.prefabName));
                    INonPlayerCharacter nonPlayer = _gameKernal.AddNonPlayerCharacter(_nonPlayerInfoList[i].data.name, new NonPlayerCharacterDesc(proto));
                    IStage stage = _gameKernal.GetStage();
                    nonPlayer.position = stage.GetStagePoint(_nonPlayerInfoList[i].stagePointName);
                }
            }
        }

        private NonPlayerInfo GetNonPlayerInfo(int id)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.id == id)
                    return _nonPlayerInfoList[i];
            }

            return null;
        }
    }
}