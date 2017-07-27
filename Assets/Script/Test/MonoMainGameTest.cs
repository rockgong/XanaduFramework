using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Config;
using Miscs;

namespace GameApp
{
	public class MonoMainGameTest : MonoBehaviour, ISaveLoadViewListener, IMainGameHost, ITitleSceneHost
	{
		private TitleScene _titleScene = new TitleScene();
		private MainGame.MainGame _mainGame = new MainGame.MainGame();

		private StageDatabase _stageDatabase = new StageDatabase();
		private NonPlayerDatabase _nonPlayerDatabase = new NonPlayerDatabase();
		private PropObjectDatabase _propObjectDatabase = new PropObjectDatabase();
		private CommonEventDatabase _commonEventDatabase = new CommonEventDatabase();
		private InteractCommandDatabase _interactCommandDatabase = new InteractCommandDatabase();
		private ScenarioPhaseDatabase _scenarioPhaseDatabase = new ScenarioPhaseDatabase();
		private InventoryDatabase _inventoryDatabase = new InventoryDatabase();
		private MainTransfer _mainTransfer = new MainTransfer();

		private bool _hudToggle = false;
		private string _startStageIdString = "1";
		private string _startStageStartPointName = "spawn";
		private string _startStageLookPointName = string.Empty;
		private string _scenarioIdString = "0";
		private string _scenarioSceneName = "TestScenario";
		private string _scenarioStagePointName = "spawn";

		private SaveLoadSystem _saveLoadSystem = new SaveLoadSystem();
		private SaveLoadView _saveLoadView = new SaveLoadView();

		private GeneralDialogView _generalDialogView = new GeneralDialogView();

		private bool _running = false;

		public bool suspending {get {return _mainGame.IsSuspending();} }

		private System.Action<int, SaveData> _currentSaveDataHandler = null;
		private System.Action _currentSaveViewCloseHandler = null;

		private int _saveStageId;
		private string _saveStagePointName;

		private MonoTestMemento _memento;

		public string titleViewPath;
		public string titleStagePath;
		public int startStageId;
		public string startStagePointName;
		public int startScenarioId;
		public string startScenarioSceneName;
		public string startScenarioStagePointName;
		// Use this for initialization
		void Start ()
		{
			GameObject playerProto = Resources.Load<GameObject>("Player/Player2");
			_stageDatabase.Initialize();
			_nonPlayerDatabase.Initialize();
			_propObjectDatabase.Initialize();
			_commonEventDatabase.LoadFromAsset("CommonEvent/CommonEvent");
			_interactCommandDatabase.LoadFromAsset("InteractCommand/InteractCommand");
			_scenarioPhaseDatabase.LoadFromAsset("ScenarioPhase/ScenarioPhase");
			_inventoryDatabase.Initialize();
	        _mainTransfer.Initialize();

	        _saveLoadSystem.Initialize(@"D:/", 10);
	        _saveLoadView.Initialize();
	        _saveLoadView.SetListener(this);
	        _generalDialogView.Initialize();

	        _titleScene.Initialize(titleViewPath, titleStagePath, 3, this);
			_mainGame.Initialize(playerProto, _stageDatabase, _nonPlayerDatabase, _propObjectDatabase, _commonEventDatabase, _interactCommandDatabase, _scenarioPhaseDatabase, _inventoryDatabase, _mainTransfer, this);

			_memento = GetComponent<MonoTestMemento>();

			_titleScene.Startup();
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}

		void OnGUI()
		{
			if (GUILayout.Button("v", GUILayout.Width(30)))
			{
				_hudToggle = !_hudToggle;
			}

			if (_hudToggle)
			{
				if (_running)
				{
					if (GUILayout.Button("Stop"))
					{
						_mainGame.ShutDown();
						_running = false;
					}
				}
				else
				{
					_startStageIdString = GUILayout.TextField(_startStageIdString);
					_startStageStartPointName = GUILayout.TextField(_startStageStartPointName);
					_startStageLookPointName = GUILayout.TextField(_startStageLookPointName);
					_scenarioIdString = GUILayout.TextField(_scenarioIdString);
					_scenarioSceneName = GUILayout.TextField(_scenarioSceneName);
					_scenarioStagePointName = GUILayout.TextField(_scenarioStagePointName);

					if (GUILayout.Button("Start"))
					{
						int startStageId = int.Parse(_startStageIdString);
						int scenarioId = int.Parse(_scenarioIdString);
						_mainGame.StartUp(startStageId, _startStageStartPointName, _startStageLookPointName, scenarioId, _scenarioSceneName, _scenarioStagePointName);
						_running = true;
					}
				}


				if (GUILayout.Button("DumpMem"))
				{
					_mainGame.DumpMemento(_memento);
				}

				if (GUILayout.Button("ApplyMem"))
				{
					_mainGame.ApplyMemento(_memento);
				}

				if (!_running)
				{
					GUILayout.BeginArea(new Rect(100, 100, 100, 10000));
					if (GUILayout.Button("New Game"))
					{

					}
					if (GUILayout.Button("Load Game"))
					{
						_saveLoadView.SetupSaveDataView(_saveLoadSystem);
						_saveLoadView.SwitchLabel(SaveLoadView.SaveLoadMode.Load);
						_saveLoadView.SetVisible(true);
						_currentSaveDataHandler = LoadDataHandler;
						_currentSaveViewCloseHandler = LoadCloseHandler;
					}
					GUILayout.EndArea();
				}
			}
		}

		public void OnSaveLoadButtonPressed(int index, SaveData data)
		{
			if (_currentSaveDataHandler != null)
				_currentSaveDataHandler(index, data);
		}
		public void OnBackButtonPressed()
		{
			_saveLoadView.SetVisible(false);
			if (_currentSaveViewCloseHandler != null)
				_currentSaveViewCloseHandler();
			_currentSaveDataHandler = null;
			_currentSaveViewCloseHandler = null;
		}

        public void OnRequestSaveSession(int stageId, string stagePointName)
        {
        	Debug.LogFormat("Request Save ! {0}, {1}", stageId, stagePointName);
        	_mainGame.Suspend();
			_saveLoadView.SetupSaveDataView(_saveLoadSystem);
			_saveLoadView.SwitchLabel(SaveLoadView.SaveLoadMode.Save);
        	_saveLoadView.SetVisible(true);
			_currentSaveDataHandler = SaveDataHandler;
			_currentSaveViewCloseHandler = SaveCloseHandler;
			_saveStageId = stageId;
			_saveStagePointName = stagePointName;
        }

        private void LoadDataHandler(int index, SaveData data)
        {
        	if (data != null)
        	{
        		_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.DoubleButton, "Sure ?", () =>
        		{
        			_mainTransfer.Transfer(0.5f, 0.3f, Color.white, () =>
        			{
		        		_saveLoadView.SetVisible(false);
						_mainGame.ApplyMemento(data);
						_mainGame.StartUp(data.stageId, data.stagePointName);
						_running = true;
						_currentSaveDataHandler = null;
						_currentSaveViewCloseHandler = null;
						_titleScene.Shutdown();
						_titleScene.Uninitialize();
        			});
        		});
			}	
        }

        private void SaveDataHandler(int index, SaveData data)
        {
        	SaveData newData = new SaveData();
        	_mainGame.DumpMemento(newData);
        	newData.stageId = _saveStageId;
        	newData.stagePointName = _saveStagePointName;
        	if (data != null)
        	{
        		_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.DoubleButton, "Override ?", () =>
        		{
        			_saveLoadSystem.SetSaveData(index, newData);
        			_saveLoadView.UpdateSingleSaveData(index, newData);
        			_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.SingleButton, "Complete !", () =>
        			{
			        	_mainGame.Resume();
			    		_saveLoadView.SetVisible(false);
						_currentSaveDataHandler = null;
						_currentSaveViewCloseHandler = null;
        			});
        		});
        	}
        	else
        	{
        		_saveLoadSystem.SetSaveData(index, newData);
    			_saveLoadView.UpdateSingleSaveData(index, newData);
    			_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.SingleButton, "Complete !", () =>
    			{
		        	_mainGame.Resume();
		    		_saveLoadView.SetVisible(false);
					_currentSaveDataHandler = null;
					_currentSaveViewCloseHandler = null;
    			});
        	}
        }

        private void LoadCloseHandler()
        {
			_currentSaveDataHandler = null;
			_currentSaveViewCloseHandler = null;
        	return;
        }

        private void SaveCloseHandler()
        {
			_currentSaveDataHandler = null;
			_currentSaveViewCloseHandler = null;
        	_mainGame.Resume();
        	return;
        }

        public void OnSelect(int index)
        {
        	if (index == 0)
        	{
        		_mainTransfer.Transfer(0.5f, 0.2f, Color.white, () =>
        		{
					_mainGame.StartUp(startStageId, startStagePointName, string.Empty, startScenarioId, startScenarioSceneName, startScenarioStagePointName);
					_titleScene.Shutdown();
					_titleScene.Uninitialize();
					_running = true;
        		});
        	}
        	else if (index == 1)
        	{
				_saveLoadView.SetupSaveDataView(_saveLoadSystem);
				_saveLoadView.SwitchLabel(SaveLoadView.SaveLoadMode.Load);
				_saveLoadView.SetVisible(true);
				_currentSaveDataHandler = LoadDataHandler;
				_currentSaveViewCloseHandler = LoadCloseHandler;
        	}
        	else if (index == 2)
        	{
        		Application.Quit();
        	}
        }
	}
}