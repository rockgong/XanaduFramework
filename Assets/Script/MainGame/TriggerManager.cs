using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    class TriggerInfo
    {
        public string name;
        public int stageId;
        public string stagePointName;
        public System.Action onTriggerEnter;
        public System.Action onTriggerExit;
    }

    public class TriggerManager
    {
        private IGameKernal _gameKernal;
        private List<TriggerInfo> _triggerList;

        public void Initialize(IGameKernal gameKernal)
        {
            _gameKernal = gameKernal;
            _triggerList = new List<TriggerInfo>();
        }

        public void AddTriggerInfo(string name, int stageId, string stagePointName, System.Action onTriggerEnter = null, System.Action onTriggerExit = null)
        {
            if (HasTriggerInfo(name))
                return;

            TriggerInfo newInfo = new TriggerInfo();
            newInfo.name = name;
            newInfo.stageId = stageId;
            newInfo.stagePointName = stagePointName;
            newInfo.onTriggerEnter = onTriggerEnter;
            newInfo.onTriggerExit = onTriggerExit;

            _triggerList.Add(newInfo);

            return;
        }

        public void RemoveTriggerInfo(string name)
        {
            for (int i = 0; i < _triggerList.Count; i++)
            {
                if (_triggerList[i].name == name)
                {
                    _triggerList.RemoveAt(i);
                    return;
                }
            }
        }

        public bool HasTriggerInfo(string name)
        {
            for (int i = 0; i < _triggerList.Count; i++)
            {
                if (_triggerList[i].name == name)
                    return true;
            }

            return false;
        }

        public void SetupTrigger(int stageId)
        {
            _gameKernal.ClearTrigger();
            for (int i = 0; i < _triggerList.Count; i++)
            {
                if (_triggerList[i].stageId == stageId)
                {
                    IStage stage = _gameKernal.GetStage();
                    Vector3 position = stage.GetStagePoint(_triggerList[i].stagePointName);
                    Vector3 size = stage.GetStagePointSize(_triggerList[i].stagePointName);
                    ITrigger trigger = _gameKernal.AddTrigger(_triggerList[i].name, new TriggerDesc(position, size));
                    trigger.onTriggerEnter = _triggerList[i].onTriggerEnter;
                    trigger.onTriggerExit = _triggerList[i].onTriggerExit;
                }
            }
        }
    }
}