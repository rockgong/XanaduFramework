using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
    public interface IPropObjectDatabaseEntry
    {
        int id { get; }
        string name { get; }
        string prefabName { get; }
    }

    public interface IPropObjectDatabase
    {
        IPropObjectDatabaseEntry GetEntryById(int id);
        List<IPropObjectDatabaseEntry> GetEntryList();
    }

    class PropObjectInfo
    {
        public IPropObjectDatabaseEntry data;
        public int stageId;
        public string stagePointName;
        public string stageLookPointName;
        public string animationStateName;
        public int scenarioId = -1;
        public string scenarioSceneName = null;
        public string scenarioStagePointName = null;
        public int interactCommandId;
    }

    interface IPropObjectManagerListener
    {
        void OnPropObjectPositionChanged(PropObjectInfo info);
        void OnPropObjectAnimationStateChanged(PropObjectInfo info);
    }

    class PropObjectManager
    {
        private List<PropObjectInfo> _propObjectInfoList = new List<PropObjectInfo>();
        private List<IPropObjectManagerListener> _listeners = new List<IPropObjectManagerListener>();
        private IGameKernal _gameKernal;

        public void Initialize(IPropObjectDatabase db, IGameKernal gameKernal)
        {
            List<IPropObjectDatabaseEntry> dataList = db.GetEntryList();
            _propObjectInfoList.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                PropObjectInfo info = new PropObjectInfo();
                info.data = dataList[i];
                info.stageId = 0;
                info.stagePointName = string.Empty;
                _propObjectInfoList.Add(info);
            }
            _gameKernal = gameKernal;
        }

        public void RegisterListener(IPropObjectManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    return;
            }

            _listeners.Add(listener);
        }

        public void UnregisterListener(IPropObjectManagerListener listener)
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

        public void SetPropObjectPosition(int id, int stageId, string stagePointName, string stageLookPointName)
        {
            PropObjectInfo info = GetPropObjectInfo(id);
            if (info != null)
            {
                if (info.stageId == stageId && info.stagePointName == stagePointName)
                    return;
                info.stageId = stageId;
                info.stagePointName = stagePointName;
                info.stageLookPointName = stageLookPointName;
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnPropObjectPositionChanged(info);
            }
        }

        public void SetPropObjectPosition(int id, int stageId, string stagePointName)
        {
            SetPropObjectPosition(id, stageId, stagePointName, string.Empty);

            return;
        }
        public void SetPropObjectAnimationStateName(int id, string animationStateName)
        {
            PropObjectInfo info = GetPropObjectInfo(id);
            if (info != null)
            {
                if (info.animationStateName == animationStateName)
                    return;

                info.animationStateName = animationStateName;
                for (int i = 0; i < _listeners.Count; i++)
                    _listeners[i].OnPropObjectAnimationStateChanged(info);
            }
        }

        public void SetPropObjectScenario(int id, int scenarioId, string sceneName, string stagePointName)
        {
            PropObjectInfo info = GetPropObjectInfo(id);
            if (info != null)
            {
                info.scenarioId = scenarioId;
                info.scenarioSceneName = sceneName;
                info.scenarioStagePointName = stagePointName;
            }
        }

        public int GetPropObjectScenarioIdByName(string name)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.name == name)
                    return _propObjectInfoList[i].scenarioId;
            }

            return -1;
        }
        public string GetPropObjectScenarioSceneNameByName(string name)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.name == name)
                    return _propObjectInfoList[i].scenarioSceneName;
            }

            return null;
        }
        public string GetPropObjectScenarioStagePointNameByName(string name)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.name == name)
                    return _propObjectInfoList[i].scenarioStagePointName;
            }

            return null;
        }

        public void SetupAllPropObjects(int stageId)
        {
            _gameKernal.ClearPropObject();
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].stageId == stageId)
                {
                    GameObject proto = Resources.Load<GameObject>(string.Format("PropObject/{0}", _propObjectInfoList[i].data.prefabName));
                    if (proto == null)
                        Debug.LogError(string.Format("Missing PropObject player : {0}", _propObjectInfoList[i].data.prefabName));
                    IPropObject propObj = _gameKernal.AddPropObject(_propObjectInfoList[i].data.name, new PropObjectDesc(proto));
                    IStage stage = _gameKernal.GetStage();
                    propObj.position = stage.GetStagePoint(_propObjectInfoList[i].stagePointName);
                    if (!string.IsNullOrEmpty(_propObjectInfoList[i].stageLookPointName))
                    {
                        Vector3 lookPoint = stage.GetStagePoint(_propObjectInfoList[i].stageLookPointName);
                        Vector3 lookVector = lookPoint - propObj.position;
                        float yaw = MathHelper.Vector3ToYaw(lookVector);
                        propObj.yaw = yaw;
                    }
                    if (!string.IsNullOrEmpty(_propObjectInfoList[i].animationStateName))
                        propObj.PlayAnimation(_propObjectInfoList[i].animationStateName);
                }
            }
        }

        public int GetInteractCommandIdByName(string name)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.name == name)
                    return _propObjectInfoList[i].interactCommandId;
            }

            return 0;
        }

        public void SetInteractCommandIdByName(int id, int commandId)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.id == id)
                    _propObjectInfoList[i].interactCommandId = commandId;
            }
        }

        private PropObjectInfo GetPropObjectInfo(int id)
        {
            for (int i = 0; i < _propObjectInfoList.Count; i++)
            {
                if (_propObjectInfoList[i].data.id == id)
                    return _propObjectInfoList[i];
            }

            return null;
        }
    }
}