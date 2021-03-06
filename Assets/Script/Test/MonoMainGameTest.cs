﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Config;
using Miscs;
using Audio;
using GameKernal;

namespace GameApp
{
	public class MonoMainGameTest : MonoBehaviour, ISaveLoadViewListener, IMainGameHost, ITitleSceneHost
	{
		private TitleScene _titleScene = new TitleScene();
		private ResultScene _resultScene = new ResultScene();
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

		public string[] titleViewPath;
		public string[] titleStagePath;
		public int startStageId;
		public string startStagePointName;
		public int startScenarioId;
		public string startScenarioSceneName;
		public string startScenarioStagePointName;
		public int[] startInventoryIds;
		public string[] resultViewPathList;
		public int valueStringCap;
		public int valueIntCap;

		private float _totalPlayedTime = 0.0f;
		private MonoDelegate _totalPlayTimeDelegate = null;

		private bool _inTransfer = false;
		// Use this for initialization
		void Start ()
		{
			TextMap.Initialize();
			GameObject playerProto = Resources.Load<GameObject>("Player/Player2");
			_stageDatabase.Initialize();
			_nonPlayerDatabase.Initialize();
			_propObjectDatabase.Initialize();
			_commonEventDatabase.LoadFromAsset("CommonEvent/CommonEvent");
			_interactCommandDatabase.LoadFromAsset("InteractCommand/InteractCommand");
			_scenarioPhaseDatabase.LoadFromAsset("ScenarioPhase/ScenarioPhase");
			_inventoryDatabase.Initialize();
	        _mainTransfer.Initialize();

	        Debug.Log(Application.persistentDataPath);
	        _saveLoadSystem.Initialize(@"D:/", 5);
	        _saveLoadView.Initialize();
	        _saveLoadView.SetListener(this);
	        _generalDialogView.Initialize();

	        _titleScene.Initialize(titleViewPath[GetTitleIndex()], titleStagePath[GetTitleIndex()], 3, this);
			_mainGame.Initialize(playerProto, _stageDatabase, _nonPlayerDatabase, _propObjectDatabase, _commonEventDatabase, _interactCommandDatabase, _scenarioPhaseDatabase, _inventoryDatabase, _mainTransfer, this, "UI/Inline", valueStringCap, valueIntCap);

			_memento = GetComponent<MonoTestMemento>();

			_titleScene.Startup();
			BGM.Play("Title");
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

		private void StartPlayTime(float offsetTime)
		{
			if (_totalPlayTimeDelegate != null)
				GameObject.Destroy(_totalPlayTimeDelegate.gameObject);

			_totalPlayedTime = offsetTime;
			MonoDelegate.Create(PlayTimeHandler, "_PlayTime");
		}

		private void StopPlayTime()
		{
			if (_totalPlayTimeDelegate != null)
			{
				GameObject.Destroy(_totalPlayTimeDelegate.gameObject);
				_totalPlayTimeDelegate = null;
			}
		}

		private void PlayTimeHandler()
		{
			_totalPlayedTime += Time.unscaledDeltaTime;
		}

		private int GetTitleIndex()
		{
			if (titleViewPath.Length == 0 || titleStagePath.Length == 0)
				return 0;

			SystemSaveData data = _saveLoadSystem.GetSystemSaveData();
			return Mathf.Clamp(data.completion, 0, Mathf.Min(titleViewPath.Length, titleStagePath.Length));
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
        	_mainGame.Suspend();
			_saveLoadView.SetupSaveDataView(_saveLoadSystem);
			_saveLoadView.SwitchLabel(SaveLoadView.SaveLoadMode.Save);
        	_saveLoadView.SetVisible(true);
			_currentSaveDataHandler = SaveDataHandler;
			_currentSaveViewCloseHandler = SaveCloseHandler;
			_saveStageId = stageId;
			_saveStagePointName = stagePointName;
        }

        public void OnRequestBackToMain()
        {
        	_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.DoubleButton, TextMap.Map("1004"), () =>
        	{
        		_mainTransfer.Transfer(0.5f, 0.3f, Color.white, () =>
        		{
        			StopPlayTime();
	        		_mainGame.ShutDown();
			        _titleScene.Initialize(titleViewPath[GetTitleIndex()], titleStagePath[GetTitleIndex()], 3, this);
	        		_titleScene.Startup();
	        		BGM.Play("Title");
        		});
        	});
        }

        private void LoadDataHandler(int index, SaveData data)
        {
        	if (data != null)
        	{
        		_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.DoubleButton, TextMap.Map("1001"), () =>
        		{
        			_mainTransfer.Transfer(0.5f, 0.3f, Color.white, () =>
        			{
		        		_saveLoadView.SetVisible(false);
						_mainGame.ApplyMemento(data);
						StartPlayTime((float)data.playedTime);
						_mainGame.StartUp(data.stageId, data.stagePointName);
						BGM.Play("MainGame");
						_running = true;
						_currentSaveDataHandler = null;
						_currentSaveViewCloseHandler = null;
						_titleScene.Shutdown();
						_titleScene.Uninitialize();
						_inTransfer = false;
        			});
        			_inTransfer = true;
        		});
			}	
        }

        private void SaveDataHandler(int index, SaveData data)
        {
        	SaveData newData = new SaveData();
        	_mainGame.DumpMemento(newData);
        	newData.stageId = _saveStageId;
        	newData.stagePointName = _saveStagePointName;
        	newData.playedTime = (int)_totalPlayedTime;
        	if (data != null)
        	{
        		_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.DoubleButton, TextMap.Map("1002"), () =>
        		{
        			_saveLoadSystem.SetSaveData(index, newData);
        			_saveLoadView.UpdateSingleSaveData(index, newData);
        			SoundEffect.Play("Saved");
        			_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.SingleButton, TextMap.Map("1003"), () =>
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
        		SoundEffect.Play("Saved");
    			_generalDialogView.Open(GeneralDialogView.GeneralDialogMode.SingleButton, TextMap.Map("1003"), () =>
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
        	if (_inTransfer)
        		return;

        	if (index == 0)
        	{
        		_mainTransfer.Transfer(0.5f, 0.2f, Color.white, () =>
        		{
        			_mainGame.ResetValueManager();
        			StartPlayTime(0f);
					_mainGame.StartUp(startStageId, startStagePointName, string.Empty, startScenarioId, startScenarioSceneName, startScenarioStagePointName);
					BGM.Play("MainGame");
					for (int i = 0; i < startInventoryIds.Length; i++)
						_mainGame.AddInventory(startInventoryIds[i]);
					_titleScene.Shutdown();
					_titleScene.Uninitialize();
					_running = true;
        			_inTransfer = false;
        		});
        		_inTransfer = true;
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

        public void OnRequestResult(int index)
        {
        	int finalIndex = Mathf.Clamp(index, 0, resultViewPathList.Length - 1);
        	_resultScene.Initialize(resultViewPathList[finalIndex]);
        	// _mainTransfer.Transfer(0.5f, 0.3f, Color.red, () =>
        	// {
        		StopPlayTime();
        		_mainGame.ShutDown();
	        	_resultScene.Startup(5.0f, () =>
	        	{
	        		_mainTransfer.Transfer(0.7f, 0.6f, Color.white, () =>
	        		{
	        			_resultScene.Uninitialize();
				        _titleScene.Initialize(titleViewPath[GetTitleIndex()], titleStagePath[GetTitleIndex()], 3, this);
				        _titleScene.Startup();
				        BGM.Play("Title");
	        		});
	        	});
        	// });

			SystemSaveData systemSaveData = _saveLoadSystem.GetSystemSaveData();
			if (index > systemSaveData.completion)
			{
				SystemSaveData newSystemSaveData = new SystemSaveData();
				newSystemSaveData.completion = index;
				_saveLoadSystem.SetSystemSaveData(newSystemSaveData);
			}
        }
	}
}