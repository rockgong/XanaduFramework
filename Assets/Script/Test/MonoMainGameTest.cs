using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Config;
using Miscs;

namespace GameApp
{
	public class MonoMainGameTest : MonoBehaviour, ISaveLoadViewListener
	{
		private MainGame.MainGame _mainGame = new MainGame.MainGame();

		private StageDatabase _stageDatabase = new StageDatabase();
		private NonPlayerDatabase _nonPlayerDatabase = new NonPlayerDatabase();
		private PropObjectDatabase _propObjectDatabase = new PropObjectDatabase();
		private CommonEventDatabase _commonEventDatabase = new CommonEventDatabase();
		private InteractCommandDatabase _interactCommandDatabase = new InteractCommandDatabase();
		private ScenarioPhaseDatabase _scenarioPhaseDatabase = new ScenarioPhaseDatabase();
		private InventoryDatabase _inventoryDatabase = new InventoryDatabase();
		private MainTransfer _mainTransfer = new MainTransfer();

		private string _startStageIdString = "1";
		private string _startStageStartPointName = "spawn";
		private string _startStageLookPointName = string.Empty;

		private SaveLoadSystem _saveLoadSystem = new SaveLoadSystem();
		private SaveLoadView _saveLoadView = new SaveLoadView();

		private bool _running = false;

		private MonoTestMemento _memento;
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

			_mainGame.Initialize(playerProto, _stageDatabase, _nonPlayerDatabase, _propObjectDatabase, _commonEventDatabase, _interactCommandDatabase, _scenarioPhaseDatabase, _inventoryDatabase, _mainTransfer);

			_memento = GetComponent<MonoTestMemento>();
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}

		void OnGUI()
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

				if (GUILayout.Button("Start"))
				{
					int startStageId = int.Parse(_startStageIdString);
					_mainGame.StartUp(startStageId, _startStageStartPointName, _startStageLookPointName);
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
					_saveLoadView.SetVisible(true);
				}
				GUILayout.EndArea();
			}
		}

		public void OnSaveLoadButtonPressed(SaveData data)
		{
			if (data != null)
			{
				_saveLoadView.SetVisible(false);
				_mainGame.ApplyMemento(data);
				_mainGame.StartUp(data.stageId, data.stagePointName);
				_running = true;
			}
		}
		public void OnBackButtonPressed()
		{
			_saveLoadView.SetVisible(false);
		}
	}
}