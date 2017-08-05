using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

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
        public string stageLookPointName;
        public string animationStateName;
        public int scenarioId = -1;
        public string scenarioSceneName = null;
        public string scenarioStagePointName = null;
        public int interactCommandId;
        public int orderCode = 1; // 1 : interact; 2 : scenario; 3 : interact -> scenario; 4 : scenario -> interact
    }

    interface INonPlayerManagerListener
    {
        void OnNonPlayerPositionChanged(NonPlayerInfo info);
        void OnNonPlayerAnimationStateChanged(NonPlayerInfo info);
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

        public void SetNonPlayerPosition(int id, int stageId, string stagePointName, string stageLookPointName)
        {
            NonPlayerInfo info = GetNonPlayerInfo(id);
            if (info != null)
            {
                if (info.stageId == stageId && info.stagePointName == stagePointName)
                    return;
                info.stageId = stageId;
                info.stagePointName = stagePointName;
                info.stageLookPointName = stageLookPointName;
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnNonPlayerPositionChanged(info);
            }
        }

        public void SetNonPlayerPosition(int id, int stageId, string stagePointName)
        {
            SetNonPlayerPosition(id, stageId, stagePointName, string.Empty);

            return;
        }

        public void SetNonPlayerAnimationStateName(int id, string animationStateName)
        {
            NonPlayerInfo info = GetNonPlayerInfo(id);
            if (info != null)
            {
                if (info.animationStateName == animationStateName)
                    return;

                info.animationStateName = animationStateName;
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnNonPlayerAnimationStateChanged(info);
            }
        }

        public void SetNonPlayerScenario(int id, int scenarioId, string sceneName, string stagePointName)
        {
            NonPlayerInfo info = GetNonPlayerInfo(id);
            if (info != null)
            {
                info.scenarioId = scenarioId;
                info.scenarioSceneName = sceneName;
                info.scenarioStagePointName = stagePointName;
            }
        }

        public int GetNonPlayerScenarioIdByName(string name)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.name == name)
                    return _nonPlayerInfoList[i].scenarioId;
            }

            return -1;
        }
        public string GetNonPlayerScenarioSceneNameByName(string name)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.name == name)
                    return _nonPlayerInfoList[i].scenarioSceneName;
            }

            return null;
        }
        public string GetNonPlayerScenarioStagePointNameByName(string name)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.name == name)
                    return _nonPlayerInfoList[i].scenarioStagePointName;
            }

            return null;
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
                    if (!string.IsNullOrEmpty(_nonPlayerInfoList[i].stageLookPointName))
                    {
                        Vector3 lookPoint = stage.GetStagePoint(_nonPlayerInfoList[i].stageLookPointName);
                        Vector3 lookVector = lookPoint - nonPlayer.position;
                        float yaw = MathHelper.Vector3ToYaw(lookVector);
                        nonPlayer.yaw = yaw;
                    }
                    if (!string.IsNullOrEmpty(_nonPlayerInfoList[i].animationStateName))
                        nonPlayer.PlayAnimation(_nonPlayerInfoList[i].animationStateName);
                }
            }
        }

        public int GetInteractCommandIdByName(string name)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.name == name)
                    return _nonPlayerInfoList[i].interactCommandId;
            }

            return 0;
        }

        public void SetInteractCommandIdByName(int id, int commandId)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.id == id)
                    _nonPlayerInfoList[i].interactCommandId = commandId;
            }
        }

        public int GetOrderCodeIdByName(string name)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.name == name)
                    return _nonPlayerInfoList[i].orderCode;
            }

            return 0;
        }

        public void SetOrderCodeIdByName(int id, int oc)
        {
            for (int i = 0; i < _nonPlayerInfoList.Count; i++)
            {
                if (_nonPlayerInfoList[i].data.id == id)
                    _nonPlayerInfoList[i].orderCode = oc;
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