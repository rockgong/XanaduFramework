using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;
using System;

namespace MainGame
{
    public class MonoGameKernalTest : MonoBehaviour, IInteractGameStateHost, IGameKernalHost, IPlayerStageManagerListener
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

        private string _swapStageId = "1";
        private string _swapPointName = "4";

        void Start()
        {
            gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);

            GameObject playerPrototype = Resources.Load<GameObject>("Player/Player");

            IPlayerCharacter player = gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

            // GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

            // IStage stage = gameKernal.SetupStage(new StageDesc(stagePrototype));
            
            TestStageDatabase stageDb = GetComponent<TestStageDatabase>();
            _playerStageManager.SetDatabase(stageDb);
            _playerStageManager.SetGameKernal(gameKernal);
            _playerStageManager.RegisterListener(this);

            TestNonPlayerDatabase nonPlayerDb = GetComponent<TestNonPlayerDatabase>();
            _nonPlayerManager.Initialize(nonPlayerDb, gameKernal);
            _nonPlayerManager.SetNonPlayerPosition(1, 1, "1");
            _nonPlayerManager.SetNonPlayerPosition(2, 2, "2");

            TestPropObjectDatabase propObjectDb = GetComponent<TestPropObjectDatabase>();
            _propObjectManager.Initialize(propObjectDb, gameKernal);
            _propObjectManager.SetPropObjectPosition(1, 1, "3");

            _triggerManager.Initialize(gameKernal);
            _triggerManager.AddTriggerInfo("swap1", 1, "2", () => _playerStageManager.SwapPlayer(2, "4"));
            _triggerManager.AddTriggerInfo("swap2", 2, "1", () => _playerStageManager.SwapPlayer(1, "4"));

            _playerStageManager.SwapPlayer(1, "4");
            /*
            INonPlayerCharacter nonPlayer = gameKernal.AddNonPlayerCharacter("nana", new NonPlayerCharacterDesc(playerPrototype));

            // nonPlayer.position = new Vector3(0.0f, 0.0f, 5.0f);
            nonPlayer.position = stage.GetStagePoint("1");

            nonPlayer = gameKernal.AddNonPlayerCharacter("nono", new NonPlayerCharacterDesc(playerPrototype));

            // nonPlayer.position = new Vector3(0.0f, 0.0f, -5.0f);
            nonPlayer.position = stage.GetStagePoint("2");

            GameObject propPrototype = Resources.Load<GameObject>("PropObject/TestProp");

            IPropObject prop = gameKernal.AddPropObject("testprop", new PropObjectDesc(propPrototype));

            // prop.position = new Vector3(0.0f, 0.0f, -10.0f);
            prop.position = stage.GetStagePoint("3");
            */

            ICamera cam = gameKernal.GetCamera();

            cam.lookPosition = player.viewPosition;

            cam.offset = cameraOffset;

            gameKernal.Startup();

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

            if (nonPlayer.name == "nono")
            {
                InteractCommandDialog dialog = new InteractCommandDialog();
                dialog.content = "OK! OK! OK! Yes";
                dialog.position = nonPlayer.position;
                commandList.Add(dialog);
                dialog = new InteractCommandDialog();
                dialog.content = "Hahahaha";
                dialog.position = nonPlayer.position;
                commandList.Add(dialog);

                InteractCommandAnimation anim = new InteractCommandAnimation();
                anim.target = CommandTarget.NonPlayer;
                anim.animationName = "Talk";
                commandList.Add(anim);

                InteractCommandWait wait = new InteractCommandWait();
                wait.time = 1.8f;
                commandList.Add(wait);

                dialog = new InteractCommandDialog();
                dialog.content = "Go Away !";
                dialog.position = nonPlayer.position;
                commandList.Add(dialog);

                InteractCommandMessage msg = new InteractCommandMessage();
                msg.content = "Warning";
                commandList.Add(msg);
                msg = new InteractCommandMessage();
                msg.content = "Boss";
                commandList.Add(msg);

                InteractCommandSelect slct = new InteractCommandSelect();
                slct.title = "Exit ?";
                slct.options = new string[] { "Yes", "No" };
                slct.callback = (i) => { if (i == 0) Application.Quit(); };
                commandList.Add(slct);
            }
            else if (nonPlayer.name == "nana")
            {
                InteractCommandNonPlayerFace face = new InteractCommandNonPlayerFace();
                commandList.Add(face);

                InteractCommandDialog dialog = new InteractCommandDialog();
                dialog.content = "Fuck";
                dialog.position = nonPlayer.position;
                commandList.Add(dialog);
            }
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
            _swapStageId = GUILayout.TextField(_swapStageId);
            _swapPointName = GUILayout.TextField(_swapPointName);

            if (GUILayout.Button("Move"))
            {
                int swapStageId = int.Parse(_swapStageId);
                _playerStageManager.SwapPlayer(swapStageId, _swapPointName);
            }
        }

        public void OnPlayerSwapped(int stageId, string stagePointName)
        {

        }

        public void OnStageChanged(int stageId)
        {
            _nonPlayerManager.SetupAllNonPlayers(stageId);
            _propObjectManager.SetupAllPropObjects(stageId);
            _triggerManager.SetupTrigger(stageId);
        }
    }
}