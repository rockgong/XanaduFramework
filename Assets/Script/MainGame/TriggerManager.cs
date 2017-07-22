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
        public int triggerType = 0; // 0 : trigger on enter (default) ; 1 : trigger on exit
        public int scenarioId = -1;
        public string scenarioSceneName = null;
        public string scenarioStagePointName = null;
        public int interactId = -1;
        public string commonEventName = null;
    }

    class TriggerManager
    {
        private IGameKernal _gameKernal;
        private InteractGameState _interactGameState;
        private InteractCommandManager _interactCommandManager;
        private ScenarioGameState _scenarioGameState;
        private ScenarioPhaseManager _scenarioPhaseManager;
        private MainGameCommandManager _mainGameCommandManager;
        private ITransfer _transfer;

        private List<TriggerInfo> _triggerList;

        private MonoScenarioScene _scene = null;

        public void Initialize(IGameKernal gameKernal, InteractGameState igs, InteractCommandManager icm, ScenarioGameState sgs, ScenarioPhaseManager spm, MainGameCommandManager mgm, ITransfer t)
        {
            _gameKernal = gameKernal;

            _interactGameState = igs;
            _interactCommandManager = icm;
            _scenarioGameState = sgs;
            _scenarioPhaseManager = spm;
            _mainGameCommandManager = mgm;
            _transfer = t;

            _triggerList = new List<TriggerInfo>();
        }

        public void AddTriggerInfo(string name, int stageId, string stagePointName, int triggerType = 0, int scenarioId = -1, string scenarioSceneName = null, string scenarioStagePointName = null, int interactId = -1, string commonEventName = null)
        {
            if (HasTriggerInfo(name))
                return;

            TriggerInfo newInfo = new TriggerInfo();
            newInfo.name = name;
            newInfo.stageId = stageId;
            newInfo.stagePointName = stagePointName;
            newInfo.triggerType = triggerType;
            newInfo.scenarioId = scenarioId;
            newInfo.scenarioSceneName = scenarioSceneName;
            newInfo.scenarioStagePointName = scenarioStagePointName;
            newInfo.interactId = interactId;
            newInfo.commonEventName = commonEventName;

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
                TriggerInfo curTrigger = _triggerList[i];
                if (curTrigger.stageId == stageId)
                {
                    IStage stage = _gameKernal.GetStage();
                    Vector3 position = stage.GetStagePoint(_triggerList[i].stagePointName);
                    Vector3 size = stage.GetStagePointSize(_triggerList[i].stagePointName);
                    ITrigger trigger = _gameKernal.AddTrigger(_triggerList[i].name, new TriggerDesc(position, size));

                    System.Action callback = () =>
                    {
                        if (curTrigger.scenarioId >= 0 && !string.IsNullOrEmpty(curTrigger.scenarioSceneName) && !string.IsNullOrEmpty(curTrigger.scenarioStagePointName))
                        {
                            GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + (curTrigger.scenarioSceneName));
                            if (proto != null)
                            {
                                Vector3 point = stage.GetStagePoint(curTrigger.scenarioStagePointName);
                                GameObject inst = GameObject.Instantiate<GameObject>(proto);
                                inst.transform.position = point;

                                BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(curTrigger.scenarioId);
                                if (phase != null)
                                {
                                    _scene = inst.GetComponent<MonoScenarioScene>();
                                    phase.Setup(_gameKernal, _scene);
                                    _scenarioGameState.Setup(_scene, phase);
                                    _transfer.Transfer(0.3f, 0.3f, Color.white, () => _gameKernal.SetGameState(_scenarioGameState));
                                }
                            }
                        }
                        else if (curTrigger.interactId >= 0)
                        {
                            BaseInteractCommand command = _interactCommandManager.GetCommandById(curTrigger.interactId);
                            if (command != null)
                            {
                                _interactGameState.player = null;
                                _interactGameState.nonPlayer = null;
                                _interactGameState.propObject = null;
                                command.Setup(_mainGameCommandManager);
                                List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();
                                commandList.Add(command);
                                _interactGameState.SetCommandList(commandList);
                                _gameKernal.SetGameState(_interactGameState);
                            }
                        }

                        if (!string.IsNullOrEmpty(curTrigger.commonEventName))
                        {
                            _mainGameCommandManager.DoCommand(curTrigger.commonEventName);
                        }
                    };

                    if (curTrigger.triggerType == 0)
                    {
                        trigger.onTriggerEnter = callback;
                        trigger.onTriggerExit = null;
                    }
                    else
                    {
                        trigger.onTriggerEnter = null;
                        trigger.onTriggerExit = callback;
                    }
                }
            }
        }

        public void TryRemoveScenarioScene()
        {
            Debug.Log("TryRemoveScenarioScene Called");
            if (_scene != null)
            {
            Debug.Log("TryRemoveScenarioScene Called Inner");
                GameObject.Destroy(_scene.gameObject);
                _scene = null;
            }
        }
    }
}