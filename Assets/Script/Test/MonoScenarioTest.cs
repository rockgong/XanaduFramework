using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Miscs;
using Config;

namespace MainGame
{
	public class MonoScenarioTest : MonoBehaviour, IScenarioGameStateHost
	{
		public string[] stagePrefabNames;
		public string[] playerPrefabNames;
		public string[] propObjectPrefabNames;
		public float cameraScrollSpeed;
		public float cameraDragFactor;

		private int _stagePrefabIndex;
		private int _playerPrefabNameIndex;
		private string _playerPointName;

		private MonoScenarioScene _scene;

		class NonPlayerInfo
		{
			public string name = string.Empty;
			public int playerPrefabNameIndex;
			public string stagePointName;
			public string stageFaceingPointName;

			public INonPlayerCharacter inst;
		}

		class PropObjectInfo
		{
			public string name = string.Empty;
			public int propPrefabNameIndex;
			public string stagePointName;
			public string stageFaceingPointName;

			public IPropObject inst;
		}

		private List<NonPlayerInfo> _nonPlayerInfoList = new List<NonPlayerInfo>();
		private List<PropObjectInfo> _propObjectInfoList = new List<PropObjectInfo>();

		private IGameKernal gameKernal;
        private ScenarioGameState _scenarioGameState = new ScenarioGameState();
        private InteractView _interactView = new InteractView();
        private ScenarioPhaseBuilder _scenarioPhaseBuilder = new ScenarioPhaseBuilder();
        private ScenarioPhaseDatabase _scenarioPhaseDatabase = new ScenarioPhaseDatabase();
        private ScenarioPhaseManager _scenarioPhaseManager = new ScenarioPhaseManager();

        private Vector3 _preMousePosition = Vector3.zero;
        private string _addNonPlayerName = string.Empty;
        private string _addPropObjectName = string.Empty;

        private string _playScenarioStagePointName = string.Empty;
        private string _playScenarioSceneName = string.Empty;
        private string _playScenarioId = string.Empty;

        private bool _playing = false;

		// Use this for initialization
		void Start () {
			gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), null);
			ICamera camera = gameKernal.GetCamera();
			camera.lookPosition = Vector3.zero;
			camera.offset = new Vector3(0.0f, 200.0f, -100.0f);
			//GameObject proto = Resources.Load<GameObject>("Stage/" + stagePrefabNames[_stagePrefabIndex]);
			gameKernal.SetupStage(new StageDesc(stagePrefabNames[_stagePrefabIndex]));
			GameObject proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[_playerPrefabNameIndex]);
			gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(proto));

			_scenarioPhaseDatabase.LoadFromAsset("ScenarioPhase/ScenarioPhase");
			_scenarioPhaseBuilder.Initialize();
			_scenarioPhaseManager.Initialize(_scenarioPhaseDatabase, _scenarioPhaseBuilder);

			_scenarioGameState.Initialize(gameKernal, this);
		}

		void Update()
		{
			ICamera camera = gameKernal.GetCamera();
			if (Input.GetButton("Fire1"))
			{
				Vector3 delta = (Input.mousePosition - _preMousePosition) * cameraDragFactor * camera.offset.magnitude;
				delta = new Vector3(delta.x, 0.0f, delta.y);
				camera.lookPosition = camera.lookPosition - delta;
			}

			Vector2 scrollDelta = Input.mouseScrollDelta;
			float distance = camera.offset.magnitude;
			distance -= scrollDelta.y * Time.deltaTime * cameraScrollSpeed;
			distance = Mathf.Clamp(distance, 1.0f, 200.0f);
			camera.offset = camera.offset.normalized * distance;

			_preMousePosition = Input.mousePosition;
		}

		public void OnScenarioEnd()
		{
			if (_playing)
			{
				_playing = false;
				gameKernal.SetGameState(null);
			}

			if (_scene != null)
			{
				GameObject.Destroy(_scene.gameObject);
				_scene = null;
			}
		}

		void OnGUI()
		{
			if (_playing)
			{
				if (GUILayout.Button("x"))
				{
					_playing = false;
					gameKernal.SetGameState(null);
				}
			}
			else
			{
				GUILayout.BeginArea(new Rect(10, 10, 150, 10000));
				DrawPlayerStagePart();
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(160, 10, 150, 10000));
				DrawNonPlayerPart();
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(320, 10, 150, 10000));
				DrawPropObjectPart();
				GUILayout.EndArea();
			}
		}
		void DrawPlayerStagePart()
		{
			_stagePrefabIndex = DrawSelection(stagePrefabNames, _stagePrefabIndex, 80, (from, to)=>
			{
				gameKernal.SetupStage(new StageDesc(stagePrefabNames[to]));
			});
			_playerPrefabNameIndex = DrawSelection(playerPrefabNames, _playerPrefabNameIndex, 80, (from, to)=>
			{
				GameObject proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[to]);
				gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(proto));
			});

			_playScenarioStagePointName = GUILayout.TextField(_playScenarioStagePointName);
			_playScenarioSceneName = GUILayout.TextField(_playScenarioSceneName);
			_playScenarioId = GUILayout.TextField(_playScenarioId);
			if (GUILayout.Button("Play"))
			{
				IStage stage = gameKernal.GetStage();
				if (stage != null)
				{
					Vector3 point = stage.GetStagePoint(_playScenarioStagePointName);
					GameObject proto = Resources.Load<GameObject>("ScenarioScene/" + _playScenarioSceneName);
					if (proto != null)
					{
						GameObject inst = GameObject.Instantiate<GameObject>(proto);
						inst.transform.position = point;

						BaseScenarioPhase phase = _scenarioPhaseManager.GetPhaseById(int.Parse(_playScenarioId));
						if (phase != null)
						{
							_scene = inst.GetComponent<MonoScenarioScene>();
							phase.Setup(gameKernal, _scene, null);
							_scenarioGameState.Setup(_scene, phase);
							gameKernal.SetGameState(_scenarioGameState);
							_playing = true;
						}
					}
				}
			}
		}

		void DrawNonPlayerPart()
		{
			int removeIndex = -1;
			for (int i = 0; i < _nonPlayerInfoList.Count; i++)
			{
				_nonPlayerInfoList[i].name = GUILayout.TextField(_nonPlayerInfoList[i].name);
				_nonPlayerInfoList[i].playerPrefabNameIndex = DrawSelection(playerPrefabNames, _nonPlayerInfoList[i].playerPrefabNameIndex, 80, (from, to)=>
				{
					if (_nonPlayerInfoList[i].inst != null)
					{
						gameKernal.RemoveNonPlayerCharacter(_nonPlayerInfoList[i].inst);
						_nonPlayerInfoList[i].inst = null;
					}
					GameObject proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[to]);
					if (proto != null)
						_nonPlayerInfoList[i].inst = gameKernal.AddNonPlayerCharacter(_nonPlayerInfoList[i].name, new NonPlayerCharacterDesc(proto));
				});
				if (GUILayout.Button("x"))
				{
					removeIndex = i;
				}
				GUILayout.Label("========");
			}
			if (removeIndex >= 0 && removeIndex < _nonPlayerInfoList.Count)
			{
				gameKernal.RemoveNonPlayerCharacter(_nonPlayerInfoList[removeIndex].inst);
				_nonPlayerInfoList.RemoveAt(removeIndex);
			}
			GUILayout.BeginHorizontal();
			_addNonPlayerName = GUILayout.TextField(_addNonPlayerName);
			if (GUILayout.Button("Add"))
			{
				NonPlayerInfo info = new NonPlayerInfo();
				info.name = _addNonPlayerName;
				GameObject proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[0]);
				if (proto != null)
					info.inst = gameKernal.AddNonPlayerCharacter(info.name, new NonPlayerCharacterDesc(proto));
				_nonPlayerInfoList.Add(info);
			}
			GUILayout.EndHorizontal();
		}

		void DrawPropObjectPart()
		{
			int removeIndex = -1;
			for (int i = 0; i < _propObjectInfoList.Count; i++)
			{
				_propObjectInfoList[i].name = GUILayout.TextField(_propObjectInfoList[i].name);
				_propObjectInfoList[i].propPrefabNameIndex = DrawSelection(propObjectPrefabNames, _propObjectInfoList[i].propPrefabNameIndex, 80, (from, to)=>
				{
					if (_propObjectInfoList[i].inst != null)
					{
						gameKernal.RemovePropObject(_propObjectInfoList[i].inst);
						_propObjectInfoList[i].inst = null;
					}
					GameObject proto = Resources.Load<GameObject>("Player/" + propObjectPrefabNames[to]);
					if (proto != null)
						_propObjectInfoList[i].inst = gameKernal.AddPropObject(_propObjectInfoList[i].name, new PropObjectDesc(proto));
				});
				if (GUILayout.Button("x"))
				{
					removeIndex = i;
				}
				GUILayout.Label("========");
			}
			if (removeIndex >= 0 && removeIndex < _propObjectInfoList.Count)
			{
				gameKernal.RemovePropObject(_propObjectInfoList[removeIndex].inst);
				_propObjectInfoList.RemoveAt(removeIndex);
			}
			GUILayout.BeginHorizontal();
			_addPropObjectName = GUILayout.TextField(_addPropObjectName);
			if (GUILayout.Button("Add"))
			{
				PropObjectInfo info = new PropObjectInfo();
				info.name = _addNonPlayerName;
				GameObject proto = Resources.Load<GameObject>("PropObject/" + propObjectPrefabNames[0]);
				if (proto != null)
					info.inst = gameKernal.AddPropObject(info.name, new PropObjectDesc(proto));
				_propObjectInfoList.Add(info);
			}
			GUILayout.EndHorizontal();
		}

		int DrawSelection(string[] selection, int index, int width = 80, System.Action<int, int> onChanged = null)
		{
			GUILayout.BeginHorizontal();
			int result = index;
			if (GUILayout.Button("<", GUILayout.Width(25)))
			{
				result = Mathf.Clamp(result - 1, 0, selection.Length - 1);
				if (result != index && onChanged != null)
					onChanged(index, result);
			}
			GUILayout.TextField(selection[index], GUILayout.Width(width));
			if (GUILayout.Button(">", GUILayout.Width(25)))
			{
				result = Mathf.Clamp(result + 1, 0, selection.Length - 1);
				if (result != index && onChanged != null)
					onChanged(index, result);
			}
			GUILayout.EndHorizontal();
			return result;
		}
	}
}