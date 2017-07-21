﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;
using System;
using Config;
using Miscs;
using UIUtil;

namespace MainGame
{
    public class MonoGameKernalTest : MonoBehaviour, IInteractGameStateHost, IGameKernalHost, IPlayerStageManagerListener, IValueManagerListener, IScenarioGameStateHost
    {
        public Vector3 cameraOffset;
        public string[] selectOptions;

        private IGameKernal gameKernal;

        private MainGameState _mainGameState = new MainGameState();
        private InteractGameState _interactGameState = new InteractGameState();
        private ScenarioGameState _scenarioGameState = new ScenarioGameState();
        private InteractView _interactView = new InteractView();
        private PlayerStageManager _playerStageManager = new PlayerStageManager();
        private NonPlayerManager _nonPlayerManager = new NonPlayerManager();
        private PropObjectManager _propObjectManager = new PropObjectManager();
        private TriggerManager _triggerManager = new TriggerManager();
        private MainGameCommandManager _mainGameCommandManager = new MainGameCommandManager();
        private MainGameCommandBuilder _mainGameCommandBuilder = new MainGameCommandBuilder();
        private InteractCommandManager _interactCommandManager = new InteractCommandManager();
        private InteractCommandBuilder _interactCommandBuilder = new InteractCommandBuilder();
        private ValueManager _valueManager = new ValueManager();
        private MainGameCameraController _mainGameCameraController = new MainGameCameraController();
        private StageDatabase _stageDatabase = new StageDatabase();
        private CommonVector3Builder _commonVector3Builder = new CommonVector3Builder();
        private MainTransfer _mainTransfer = new MainTransfer();
        private ScenarioPhaseBuilder _scenarioPhaseBuilder = new ScenarioPhaseBuilder();
        private ScenarioPhaseDatabase _scenarioPhaseDatabase = new ScenarioPhaseDatabase();
        private ScenarioPhaseManager _scenarioPhaseManager = new ScenarioPhaseManager();

        private MonoScenarioScene _scenarioScene = null;
        private List<ITrigger> _stageTransferTriggers = new List<ITrigger>();

        private string _sceneName = "TestScenario";
        private string _scenarioId = "1";

        void Start()
        {
            TextMap.Initialize();
            
            gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);

            GameObject playerPrototype = Resources.Load<GameObject>("Player/Player2");

            IPlayerCharacter player = gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

            // GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

            // IStage stage = gameKernal.SetupStage(new StageDesc(stagePrototype));
            
            _commonVector3Builder.Initialize();
            _stageDatabase = new StageDatabase();
            _stageDatabase.Initialize();
            _playerStageManager.SetDatabase(_stageDatabase);
            _playerStageManager.SetGameKernal(gameKernal);
            _playerStageManager.RegisterListener(this);

            NonPlayerDatabase nonPlayerDb = new NonPlayerDatabase();
            nonPlayerDb.Initialize();
            _nonPlayerManager.Initialize(nonPlayerDb, gameKernal);
            _nonPlayerManager.SetNonPlayerPosition(1, 1, "npc_1");
            _nonPlayerManager.SetNonPlayerPosition(2, 1, "npc_2", "npc_2_look");
            _nonPlayerManager.SetNonPlayerPosition(3, 2, "3");
            _nonPlayerManager.SetNonPlayerAnimationStateName(1, "Run");
            // _nonPlayerManager.SetNonPlayerPosition(4, 2, "1");

            PropObjectDatabase propObjectDb = new PropObjectDatabase();
            propObjectDb.Initialize();
            _propObjectManager.Initialize(propObjectDb, gameKernal);
            _propObjectManager.SetPropObjectPosition(1, 1, "3");
            _propObjectManager.SetPropObjectPosition(2, 3, "monitor");
            _propObjectManager.SetPropObjectPosition(3, 3, "locker_1");
            _propObjectManager.SetPropObjectPosition(4, 3, "locker_2");
            _propObjectManager.SetInteractCommandIdByName(2, 4);
            _propObjectManager.SetInteractCommandIdByName(3, 5);
            _propObjectManager.SetInteractCommandIdByName(4, 6);

            _triggerManager.Initialize(gameKernal);

            _valueManager = new ValueManager();
            _valueManager.Initialize(256, 256);
            _valueManager.RegisterListener(this);

            CommonEventDatabase commonEventDatabase = new CommonEventDatabase();
            commonEventDatabase.LoadFromAsset("CommonEvent/CommonEvent");
            _mainGameCommandBuilder.Initialize();
            // _mainGameCommandManager.Initialize(gameKernal, _playerStageManager, _nonPlayerManager, _propObjectManager, _triggerManager, _mainGameCommandBuilder, GetComponent<TestCommonEventDatabase>(), _valueManager);
            _mainGameCommandManager.Initialize(gameKernal, _playerStageManager, _nonPlayerManager, _propObjectManager, _triggerManager, _mainGameCommandBuilder, commonEventDatabase, _valueManager);
            InteractCommandDatabase interactCommandDatabase = new InteractCommandDatabase();
            interactCommandDatabase.LoadFromAsset("InteractCommand/InteractCommand");
            _interactCommandBuilder.Initialize();
            // _interactCommandManager.Initialize(_interactGameState, GetComponent<TestInteractCommandDatabase>(), _interactCommandBuilder);
            _interactCommandManager.Initialize(_interactGameState, interactCommandDatabase, _interactCommandBuilder);

            _scenarioPhaseDatabase.LoadFromAsset("ScenarioPhase/ScenarioPhase");
            _scenarioPhaseBuilder.Initialize();
            _scenarioPhaseManager.Initialize(_scenarioPhaseDatabase, _scenarioPhaseBuilder);

            _mainGameCommandManager.DoCommand("Change1");
            _mainGameCommandManager.DoCommand("Change3");

            ICamera cam = gameKernal.GetCamera();

            cam.lookPosition = player.viewPosition;

            cam.offset = cameraOffset;

            gameKernal.Startup();

            _mainGameCameraController.Initialize(gameKernal.GetCamera());

            _playerStageManager.SwapPlayer(3, "spawn");

            _mainGameState.SetCameraController(_mainGameCameraController);

            gameKernal.SetGameState(_mainGameState);

            _interactGameState.SetHost(this);
            _scenarioGameState.Initialize(gameKernal, this);

            _interactView.Initialize();

            _interactGameState.SetInteractView(_interactView);
            _interactView.SetListener(_interactGameState);
            _mainTransfer.Initialize();

        }

        public void OnCommandProcessEnd()
        {
            gameKernal.SetGameState(_mainGameState);
        }

        public void OnInteract(IPlayerCharacter player, INonPlayerCharacter nonPlayer)
        {
            _interactGameState.player = player;
            _interactGameState.nonPlayer = nonPlayer;
            _interactGameState.propObject = null;

            //Temp Code
            List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();

            int interactCommandId = _nonPlayerManager.GetInteractCommandIdByName(nonPlayer.name);
            BaseInteractCommand command = _interactCommandManager.GetCommandById(interactCommandId);
            command.Setup(_mainGameCommandManager);
            commandList.Add(command);
            _interactGameState.SetCommandList(commandList);

            gameKernal.SetGameState(_interactGameState);
        }

        public void OnInteract(IPlayerCharacter player, IPropObject prop)
        {
            _interactGameState.player = player;
            _interactGameState.nonPlayer = null;
            _interactGameState.propObject = prop;

            //Temp Code
            List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();

            int interactCommandId = _propObjectManager.GetInteractCommandIdByName(prop.name);
            BaseInteractCommand command = _interactCommandManager.GetCommandById(interactCommandId);
            command.Setup(_mainGameCommandManager);
            commandList.Add(command);
            _interactGameState.SetCommandList(commandList);

            gameKernal.SetGameState(_interactGameState);
        }

        public void OnReadyToInteractChanged(INonPlayerCharacter from, INonPlayerCharacter to)
        {
            Debug.LogFormat("INonPlayerCharacter => {0}, INonPlayerCharacter => {1}", from == null ? "<NULL>" : from.name, to == null ? "<NULL>" : to.name);
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(INonPlayerCharacter from, IPropObject to)
        {
            Debug.LogFormat("INonPlayerCharacter => {0}, IPropObject => {1}", from == null ? "<NULL>" : from.name, to == null ? "<NULL>" : to.name);
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(IPropObject from, INonPlayerCharacter to)
        {
            Debug.LogFormat("IPropObject => {0}, INonPlayerCharacter => {1}", from == null ? "<NULL>" : from.name, to == null ? "<NULL>" : to.name);
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(IPropObject from, IPropObject to)
        {
            Debug.LogFormat("IPropObject => {0}, IPropObject => {1}", from == null ? "<NULL>" : from.name, to == null ? "<NULL>" : to.name);
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }


        private void OnGUI()
        {
            /*
            _sceneName = GUILayout.TextField(_sceneName);
            _scenarioId = GUILayout.TextField(_scenarioId);
            if (GUILayout.Button("Excute"))
            {
                int scenarioId = int.Parse(_scenarioId);
                BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(scenarioId);
                GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + _sceneName);
                GameObject inst = GameObject.Instantiate<GameObject>(proto);
                _scenarioScene = inst.GetComponent<MonoScenarioScene>();
                phase.Setup(gameKernal, _scenarioScene);
                _scenarioGameState.Setup(_scenarioScene, phase);

                _mainTransfer.Transfer(0.3f, 0.3f, Color.white, () => gameKernal.SetGameState(_scenarioGameState));
            }
            */
        }

        public void OnPlayerSwapped(int stageId, string stagePointName)
        {
            _mainGameCameraController.CancelEasing();
        }

        public void OnStageChanged(int stageId)
        {
            Debug.Log("Stage Changed " + stageId.ToString());

            _nonPlayerManager.SetupAllNonPlayers(stageId);
            _propObjectManager.SetupAllPropObjects(stageId);
            _triggerManager.SetupTrigger(stageId);
            IStageDatabaseEntry stageEntry = _stageDatabase.GetEntryById(stageId);
            _mainGameCameraController.cameraTarget = _commonVector3Builder.Build(stageEntry.cameraLook, _commonVector3Builder);
            _mainGameCameraController.cameraTarget.Setup(gameKernal);
            _mainGameCameraController.cameraPosition = _commonVector3Builder.Build(stageEntry.cameraPos, _commonVector3Builder);
            _mainGameCameraController.cameraPosition.Setup(gameKernal);

            for (int i = 0; i < _stageTransferTriggers.Count; i++)
                gameKernal.RemoveTrigger(_stageTransferTriggers[i]);
            _stageTransferTriggers.Clear();

            IStageDatabaseEntry entry = _stageDatabase.GetEntryById(stageId);
            if (entry != null && entry.transfers != null)
            {
                for (int i = 0; i < entry.transfers.Length; i++)
                {
                    StageTransferData curData = entry.transfers[i];
                    IStage stage = gameKernal.GetStage();
                    Vector3 position = stage.GetStagePoint(curData.triggerPointName);
                    Vector3 size = stage.GetStagePointSize(curData.triggerPointName);
                    ITrigger trigger = gameKernal.AddTrigger("TRANSFER_" + curData.stageId + "_" + curData.stagePointName, new TriggerDesc(position, size));
                    trigger.onTriggerEnter = () => _mainTransfer.Transfer(0.3f, 0.3f, Color.white, () =>
                    {
                        _playerStageManager.SwapPlayer(curData.stageId, curData.stagePointName);
                    });
                    trigger.onTriggerExit = null;
                    _stageTransferTriggers.Add(trigger);
                }
            }
        }

        public void OnValueChanged(int type, int index)
        {
            _mainGameCommandManager.DoCommand("Update");
            Debug.Log("OnValueChanged Called");

            return;
        }

        public void OnScenarioEnd()
        {
            _mainTransfer.Transfer(0.3f, 0.3f, Color.white, () => 
            {
                gameKernal.SetGameState(_mainGameState);
                if (_scenarioScene != null)
                {
                    GameObject.Destroy(_scenarioScene.gameObject);
                    _scenarioScene = null;
                }
            });
        }
    }
}