using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public interface IMainGameMemento
    {
        string[] stringValues{get;set;}
        int[] intValues{get;set;}
        int[] inventoryIds{get;set;}
    }

    public interface IMainGameHost
    {
        void OnRequestSaveSession(int stageId, string stagePointName);
        void OnRequestBackToMain();
        void OnRequestResult(int index);
        bool suspending{get;}
    }

	public class MainGame : IInteractGameStateHost, IGameKernalHost, IPlayerStageManagerListener, IValueManagerListener, IScenarioGameStateHost, IMainGameViewListener, IMenuViewListener, IInventoryManagerListener
	{
        private IGameKernal _gameKernal;
        private MainGameState _mainGameState;
        private MainGameView _mainGameView;
        private MenuGameState _menuGameState;
        private MenuView _menuView;
        private InteractGameState _interactGameState;
        private ScenarioGameState _scenarioGameState;
        private InteractView _interactView;

        private PlayerStageManager _playerStageManager;
        private NonPlayerManager _nonPlayerManager;
        private PropObjectManager _propObjectManager;
        private TriggerManager _triggerManager;

        private MainGameCommandManager _mainGameCommandManager;
        private MainGameCommandBuilder _mainGameCommandBuilder;

        private InteractCommandManager _interactCommandManager;
        private InteractCommandBuilder _interactCommandBuilder;

        private ScenarioPhaseManager _scenarioPhaseManager;
        private ScenarioPhaseBuilder _scenarioPhaseBuilder;

        private CommonVector3Builder _commonVector3Builder;

        private InventoryManager _inventoryManager;

        private ValueManager _valueManager;

        private MainGameCameraController _mainGameCameraController;

        // External
        private IStageDatabase _stageDatabase;
        private INonPlayerDatabase _nonPlayerDatabase;
        private IPropObjectDatabase _propObjectDatabase;
        private ICommonEventDatabase _commonEventDatabase;
        private IInteractCommandDatabase _interactCommandDatabase;
        private IScenarioPhaseDatabase _scenarioPhaseDatabase;
        private IInventoryDatabase _inventoryDatabase;
        private ITransfer _transfer;

        private GameObject _playerProto;

        private MonoScenarioScene _scenarioScene = null;
        private List<ITrigger> _stageTransferTriggers = new List<ITrigger>();

        private bool _running = false;
        private bool _suspending = false;
        private bool _applyingMemento = false;

        private IMainGameHost _host;

		public void Initialize(GameObject playerProto, IStageDatabase stageDatabase, INonPlayerDatabase nonPlayerDatabase, IPropObjectDatabase propObjectDatabase, ICommonEventDatabase commonEventDatabase, IInteractCommandDatabase interactCommandDatabase, IScenarioPhaseDatabase scenarioPhaseDatabase, IInventoryDatabase inventoryDatabase, ITransfer transfer, IMainGameHost host, int valueStringCap = 4, int valueIntCap = 4)
		{
            _host = host;

			_gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);
			_playerProto = playerProto;

	        _mainGameState = new MainGameState();
	        _mainGameView = new MainGameView();
	    	_menuGameState = new MenuGameState();
	        _menuView = new MenuView();
	        _interactGameState = new InteractGameState();
	        _scenarioGameState = new ScenarioGameState();
	        _interactView = new InteractView();
	        _playerStageManager = new PlayerStageManager();
	        _nonPlayerManager = new NonPlayerManager();
	        _propObjectManager = new PropObjectManager();
	        _triggerManager = new TriggerManager();
	        _mainGameCommandManager = new MainGameCommandManager();
	        _mainGameCommandBuilder = new MainGameCommandBuilder();
	        _interactCommandManager = new InteractCommandManager();
	        _interactCommandBuilder = new InteractCommandBuilder();
	        _scenarioPhaseManager = new ScenarioPhaseManager();
	        _scenarioPhaseBuilder = new ScenarioPhaseBuilder();
	        _commonVector3Builder = new CommonVector3Builder();
	    	_inventoryManager = new InventoryManager();
	        _valueManager = new ValueManager();
	        _mainGameCameraController = new MainGameCameraController();

	        // Initialization
	        _mainGameState.SetCameraController(_mainGameCameraController);
	        _mainGameState.SetMainGameView(_mainGameView);
	        _mainGameCameraController.Initialize(_gameKernal.GetCamera());

            _mainGameView.Initialize();
            _mainGameView.SetVisible(false);
			_mainGameView.SetListener(this);

            _menuGameState.SetMenuView(_menuView);

            _menuView.Initialize();
            _menuView.SetVisible(false);
            _menuView.SetListener(this);

            _interactGameState.SetHost(this);
            _interactGameState.SetInteractView(_interactView);

            _scenarioGameState.Initialize(_gameKernal, this);

            _interactView.Initialize();
            _interactView.SetListener(_interactGameState);

            _playerStageManager.SetDatabase(stageDatabase);
            _playerStageManager.SetGameKernal(_gameKernal);
            _playerStageManager.RegisterListener(this);

            _nonPlayerManager.Initialize(nonPlayerDatabase, _gameKernal);
            _propObjectManager.Initialize(propObjectDatabase, _gameKernal);
            _triggerManager.Initialize(_gameKernal, _interactGameState, _interactCommandManager, _scenarioGameState, _scenarioPhaseManager, _mainGameCommandManager, transfer, _host);

            _mainGameCommandBuilder.Initialize();
            _mainGameCommandManager.Initialize(_gameKernal, _playerStageManager, _nonPlayerManager, _propObjectManager, _triggerManager, _mainGameCommandBuilder, commonEventDatabase, _valueManager, _inventoryManager, _host);

            _interactCommandBuilder.Initialize();
            _interactCommandManager.Initialize(_interactGameState, interactCommandDatabase, _interactCommandBuilder);

            _scenarioPhaseBuilder.Initialize();
            _scenarioPhaseManager.Initialize(scenarioPhaseDatabase, _scenarioPhaseBuilder);

            _commonVector3Builder.Initialize();

            _inventoryManager.Initialize(inventoryDatabase, 10);
            _inventoryManager.RegisterListener(this);

            _valueManager.Initialize(valueStringCap, valueIntCap);

        	_stageDatabase = stageDatabase;
        	_nonPlayerDatabase = nonPlayerDatabase;
        	_propObjectDatabase = propObjectDatabase;
        	_commonEventDatabase = commonEventDatabase;
        	_interactCommandDatabase = interactCommandDatabase;
        	_scenarioPhaseDatabase = scenarioPhaseDatabase;
        	_inventoryDatabase = inventoryDatabase;
        	_transfer = transfer;
		}

		public void StartUp(int stageId, string stagePointName, string stageLookPointName = null, int scenarioId = 0, string scenarioSceneName = null, string scenarioStagePointName = null)
		{
			if (_running)
				return;

			_gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(_playerProto));
			if (_playerStageManager != null)
				_playerStageManager.SwapPlayer(stageId, stagePointName, stageLookPointName);
			_gameKernal.SetGameState(_mainGameState);
			_gameKernal.Startup();

            _nonPlayerManager.Initialize(_nonPlayerDatabase, _gameKernal);
            _propObjectManager.Initialize(_propObjectDatabase, _gameKernal);
            _triggerManager.Initialize(_gameKernal, _interactGameState, _interactCommandManager, _scenarioGameState, _scenarioPhaseManager, _mainGameCommandManager, _transfer, _host);

			_mainGameCommandManager.DoCommand("Update");
			_running = true;

            if (scenarioId >= 0 && !string.IsNullOrEmpty(scenarioSceneName) && !string.IsNullOrEmpty(scenarioStagePointName))
            {
                GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + (scenarioSceneName));
                if (proto != null)
                {
                    Vector3 point = _gameKernal.GetStage().GetStagePoint(scenarioStagePointName);
                    GameObject inst = GameObject.Instantiate<GameObject>(proto);
                    inst.transform.position = point;

                    BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(scenarioId);
                    if (phase != null)
                    {
                        _scenarioScene = inst.GetComponent<MonoScenarioScene>();
                        phase.Setup(_gameKernal, _scenarioScene);
                        _scenarioGameState.Setup(_scenarioScene, phase);

                        // Make 2 frame delay to wait for scene loading
                        int frameCnt = 2;
                        MonoDelegate del = null;
                        del = MonoDelegate.Create(() =>
                        {
                            frameCnt --;
                            if (frameCnt <= 0)
                            {
                                _gameKernal.SetGameState(_scenarioGameState);
                                GameObject.Destroy(del.gameObject);
                            }
                        });
                    }
                }
            }
		}

		public void ShutDown()
		{
			if (_running)
			{
				_gameKernal.SetGameState(null);
				_gameKernal.Shutdown();
				_playerStageManager.ClearStageRecord();
				_inventoryManager.Uninitialize();
				_running = false;
			}
		}

        public void ApplyMemento(IMainGameMemento memento)
        {
            _applyingMemento = true;
            _valueManager.Initialize(_valueManager.stringValueCapacity, _valueManager.intValueCapacity);
            string[] stringValues = memento.stringValues;
            if (stringValues != null)
            {
                for (int i = 0; i < stringValues.Length; i++)
                {
                    if (i >= _valueManager.stringValueCapacity)
                        break;
                    _valueManager.SetStringValue(i, stringValues[i]);
                }
            }

            int[] intValues = memento.intValues;
            if (intValues != null)
            {
                for (int i = 0; i < intValues.Length; i++)
                {
                    if (i >= _valueManager.intValueCapacity)
                        break;
                    _valueManager.SetIntValue(i, intValues[i]);
                }
            }

            _inventoryManager.ClearInventory();
            int[] inventoryIds = memento.inventoryIds;
            if (inventoryIds != null)
            {
                for (int i = 0; i < inventoryIds.Length; i++)
                {
                    _inventoryManager.AddInventory(inventoryIds[i]);
                }
            }
            _applyingMemento = false;

            _mainGameCommandManager.DoCommand("Update");
        }

        public void DumpMemento(IMainGameMemento memento)
        {
            string[] stringValues = new string[_valueManager.stringValueCapacity];
            for (int i = 0; i < stringValues.Length; i++)
                stringValues[i] = _valueManager.GetStringValue(i);

            int[] intValues = new int[_valueManager.intValueCapacity];
            for (int i = 0; i < intValues.Length; i++)
                intValues[i] = _valueManager.GetIntValue(i);

            List<int> inventoryIdList = new List<int>(_inventoryManager.GetInventoryCount());
            _inventoryManager.ForEachInventory((info) => inventoryIdList.Add(info.data.id));

            memento.stringValues = stringValues;
            memento.intValues = intValues;
            memento.inventoryIds = inventoryIdList.ToArray();
        }

        public void Suspend()
        {
            if (_running)
            {
                if (!_suspending)
                {
                    _suspending = true;
                }
            }
        }

        public void Resume()
        {
            if (_running)
            {
                if (_suspending)
                {
                    _suspending = false;
                }
            }
        }

        public bool IsSuspending()
        {
            return _suspending;
        }

		// Interface Implementing
        public void OnCommandProcessEnd()
        {
            _gameKernal.SetGameState(_mainGameState);
        }

        public void OnInteract(IPlayerCharacter player, INonPlayerCharacter nonPlayer)
        {
            int scenarioId = _nonPlayerManager.GetNonPlayerScenarioIdByName(nonPlayer.name);
            string scenarioSceneName = _nonPlayerManager.GetNonPlayerScenarioSceneNameByName(nonPlayer.name);
            string scenerioStagePointName = _nonPlayerManager.GetNonPlayerScenarioStagePointNameByName(nonPlayer.name);
            int interactCommandId = _nonPlayerManager.GetInteractCommandIdByName(nonPlayer.name);

            if (scenarioId >= 0 && !string.IsNullOrEmpty(scenarioSceneName) && !string.IsNullOrEmpty(scenerioStagePointName))
            {
                GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + (scenarioSceneName));
                if (proto != null)
                {
                    Vector3 point = _gameKernal.GetStage().GetStagePoint(scenerioStagePointName);
                    GameObject inst = GameObject.Instantiate<GameObject>(proto);
                    inst.transform.position = point;

                    BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(scenarioId);
                    if (phase != null)
                    {
                        _scenarioScene = inst.GetComponent<MonoScenarioScene>();
                        phase.Setup(_gameKernal, _scenarioScene);
                        _scenarioGameState.Setup(_scenarioScene, phase);
                        _transfer.Transfer(0.3f, 0.3f, Color.white, () => _gameKernal.SetGameState(_scenarioGameState));
                    }
                }
            }
            else if (interactCommandId >= 0)
            {
                _interactGameState.player = player;
                _interactGameState.nonPlayer = nonPlayer;
                _interactGameState.propObject = null;

                //Temp Code
                List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();

                BaseInteractCommand command = _interactCommandManager.GetCommandById(interactCommandId);
                command.Setup(_mainGameCommandManager, _host);
                commandList.Add(command);
                _interactGameState.SetCommandList(commandList);

                _gameKernal.SetGameState(_interactGameState);
            }
        }

        public void OnInteract(IPlayerCharacter player, IPropObject prop)
        {
            int scenarioId = _propObjectManager.GetPropObjectScenarioIdByName(prop.name);
            string scenarioSceneName = _propObjectManager.GetPropObjectScenarioSceneNameByName(prop.name);
            string scenerioStagePointName = _propObjectManager.GetPropObjectScenarioStagePointNameByName(prop.name);
            int interactCommandId = _propObjectManager.GetInteractCommandIdByName(prop.name);

            if (scenarioId >= 0 && !string.IsNullOrEmpty(scenarioSceneName) && !string.IsNullOrEmpty(scenerioStagePointName))
            {
                GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + (scenarioSceneName));
                if (proto != null)
                {
                    Vector3 point = _gameKernal.GetStage().GetStagePoint(scenerioStagePointName);
                    GameObject inst = GameObject.Instantiate<GameObject>(proto);
                    inst.transform.position = point;

                    BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(scenarioId);
                    if (phase != null)
                    {
                        _scenarioScene = inst.GetComponent<MonoScenarioScene>();
                        phase.Setup(_gameKernal, _scenarioScene);
                        _scenarioGameState.Setup(_scenarioScene, phase);
                        _transfer.Transfer(0.3f, 0.3f, Color.white, () => _gameKernal.SetGameState(_scenarioGameState));
                    }
                }
            }
            else if (interactCommandId >= 0)
            {
                _interactGameState.player = player;
                _interactGameState.nonPlayer = null;
                _interactGameState.propObject = prop;

                //Temp Code
                List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();

                BaseInteractCommand command = _interactCommandManager.GetCommandById(interactCommandId);
                command.Setup(_mainGameCommandManager, _host);
                commandList.Add(command);
                _interactGameState.SetCommandList(commandList);

                _gameKernal.SetGameState(_interactGameState);
            }
        }

        public void OnReadyToInteractChanged(INonPlayerCharacter from, INonPlayerCharacter to)
        {
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(INonPlayerCharacter from, IPropObject to)
        {
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(IPropObject from, INonPlayerCharacter to)
        {
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
        }
        public void OnReadyToInteractChanged(IPropObject from, IPropObject to)
        {
            if (to != null)
                _interactView.ShowReady(to);
            else
                _interactView.CloseReady();
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
            _mainGameCameraController.cameraTarget.Setup(_gameKernal);
            _mainGameCameraController.cameraPosition = _commonVector3Builder.Build(stageEntry.cameraPos, _commonVector3Builder);
            _mainGameCameraController.cameraPosition.Setup(_gameKernal);

            for (int i = 0; i < _stageTransferTriggers.Count; i++)
                _gameKernal.RemoveTrigger(_stageTransferTriggers[i]);
            _stageTransferTriggers.Clear();

            IStageDatabaseEntry entry = _stageDatabase.GetEntryById(stageId);
            if (entry != null && entry.transfers != null)
            {
                for (int i = 0; i < entry.transfers.Length; i++)
                {
                    StageTransferData curData = entry.transfers[i];
                    IStage stage = _gameKernal.GetStage();
                    Vector3 position = stage.GetStagePoint(curData.triggerPointName);
                    Vector3 size = stage.GetStagePointSize(curData.triggerPointName);
                    ITrigger trigger = _gameKernal.AddTrigger("TRANSFER_" + curData.stageId + "_" + curData.stagePointName, new TriggerDesc(position, size));
                    trigger.onTriggerEnter = () => _transfer.Transfer(0.3f, 0.3f, Color.white, () =>
                    {
                        _playerStageManager.SwapPlayer(curData.stageId, curData.stagePointName, curData.stageLookPointName);
                    });
                    trigger.onTriggerExit = null;
                    _stageTransferTriggers.Add(trigger);
                }
            }
        }

        public void OnValueChanged(int type, int index)
        {
            if (!_applyingMemento)
                _mainGameCommandManager.DoCommand("Update");

            return;
        }

        public void OnScenarioEnd()
        {
            _transfer.Transfer(0.3f, 0.3f, Color.white, () => 
            {
                _gameKernal.SetGameState(_mainGameState);
                if (_scenarioScene != null)
                {
                    GameObject.Destroy(_scenarioScene.gameObject);
                    _scenarioScene = null;
                }

                _triggerManager.TryRemoveScenarioScene();
            });
        }

        public void OnMenuButtonPressed()
        {
            _menuView.SetupInventoryList(_inventoryManager);
            _menuView.ShowInventory(false);
            _gameKernal.SetGameState(_menuGameState);
        }

        public void OnBackButtonPressed()
        {
            _gameKernal.SetGameState(_mainGameState);
        }

        public void OnBackToMainButtonPressed()
        {
            if (_host != null)
                _host.OnRequestBackToMain();
        }

        public void OnInventoryButtonPressed(InventoryInfo info)
        {
            _interactGameState.player = null;
            _interactGameState.nonPlayer = null;
            _interactGameState.propObject = null;

            List<BaseInteractCommand> commandList = new List<BaseInteractCommand>();
            InteractCommandMessage message = new InteractCommandMessage();
            message.content = info.data.desc;
            commandList.Add(message);
            _interactGameState.SetCommandList(commandList);
            _gameKernal.SetGameState(_interactGameState);
        }

        private void OnInventoryChanged()
        {
            if (!_applyingMemento)
                _mainGameCommandManager.DoCommand("Update");
        }

        public void OnInventoryAdded(InventoryInfo info)
        {
            OnInventoryChanged();
        }
        public void OnInventoryRemoved(InventoryInfo info)
        {
            OnInventoryChanged();
        }
	}
}