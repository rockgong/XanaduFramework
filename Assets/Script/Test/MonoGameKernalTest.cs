﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;
using System;
using Config;

namespace MainGame
{
    public class MonoGameKernalTest : MonoBehaviour, IInteractGameStateHost, IGameKernalHost, IPlayerStageManagerListener, IValueManagerListener
    {
        public Vector3 cameraOffset;
        public string[] selectOptions;

        private IGameKernal gameKernal;

        private MainGameState _mainGameState = new MainGameState();
        private InteractGameState _interactGameState = new InteractGameState();
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

        private string _commonEventName = "TestEvent";

        void Start()
        {
            TextMap.Initialize();
            
            gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);

            GameObject playerPrototype = Resources.Load<GameObject>("Player/Player");

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
            _nonPlayerManager.SetNonPlayerPosition(1, 1, "1");
            _nonPlayerManager.SetNonPlayerPosition(2, 2, "2");
            _nonPlayerManager.SetNonPlayerPosition(3, 2, "3");
            // _nonPlayerManager.SetNonPlayerPosition(4, 2, "1");

            PropObjectDatabase propObjectDb = new PropObjectDatabase();
            propObjectDb.Initialize();
            _propObjectManager.Initialize(propObjectDb, gameKernal);
            _propObjectManager.SetPropObjectPosition(1, 1, "3");

            _triggerManager.Initialize(gameKernal);
            _triggerManager.AddTriggerInfo("swap1", 1, "2", () => _playerStageManager.SwapPlayer(2, "4"));
            _triggerManager.AddTriggerInfo("swap2", 2, "1", () => _playerStageManager.SwapPlayer(1, "4"));

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

            _mainGameCommandManager.DoCommand("Change1");
            _mainGameCommandManager.DoCommand("Change3");

            _playerStageManager.SwapPlayer(1, "4");

            ICamera cam = gameKernal.GetCamera();

            cam.lookPosition = player.viewPosition;

            cam.offset = cameraOffset;

            gameKernal.Startup();

            _mainGameCameraController.Initialize(gameKernal.GetCamera());

            _mainGameState.SetCameraController(_mainGameCameraController);

            gameKernal.SetGameState(_mainGameState);

            _interactGameState.SetHost(this);

            _interactView.Initialize();

            _interactGameState.SetInteractView(_interactView);
            _interactView.SetListener(_interactGameState);

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

            InteractCommandMessage msg = new InteractCommandMessage();
            msg.content = "This is a Cube";
            commandList.Add(msg);
            _interactGameState.SetCommandList(commandList);

            gameKernal.SetGameState(_interactGameState);
        }

        private void OnGUI()
        {
            _commonEventName = GUILayout.TextField(_commonEventName);
            if (GUILayout.Button("Excute"))
            {
                _mainGameCommandManager.DoCommand(_commonEventName);
            }
        }

        public void OnPlayerSwapped(int stageId, string stagePointName)
        {

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
        }

        public void OnValueChanged(int type, int index)
        {
            _mainGameCommandManager.DoCommand("Update");
            Debug.Log("OnValueChanged Called");

            return;
        }
    }
}