using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Miscs;
using Config;

namespace MainGame
{
	public class MonoScenarioTest : MonoBehaviour
	{
		public string[] stagePrefabNames;
		public string[] playerPrefabNames;
		public string[] propObjectPrefabNames;
		public float cameraScrollSpeed;
		public float cameraDragFactor;

		private int _stagePrefabIndex;
		private int _playerPrefabNameIndex;
		private string _playerPointName;

		class NonPlayerInfo
		{
			public int playerPrefabNameIndex;
			public string stagePointName;
			public string stageFaceingPointName;

			public INonPlayerCharacter inst;
		}

		class PropObjectInfo
		{
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

		// Use this for initialization
		void Start () {
			gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), null);
			ICamera camera = gameKernal.GetCamera();
			camera.lookPosition = Vector3.zero;
			camera.offset = new Vector3(0.0f, 200.0f, -100.0f);
			GameObject proto = Resources.Load<GameObject>("Stage/" + stagePrefabNames[_stagePrefabIndex]);
			gameKernal.SetupStage(new StageDesc(proto));
			proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[_playerPrefabNameIndex]);
			gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(proto));


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

		void OnGUI()
		{
			_stagePrefabIndex = DrawSelection(stagePrefabNames, _stagePrefabIndex, 80, (from, to)=>
			{
				GameObject proto = Resources.Load<GameObject>("Stage/" + stagePrefabNames[to]);
				gameKernal.SetupStage(new StageDesc(proto));
			});
			_playerPrefabNameIndex = DrawSelection(playerPrefabNames, _playerPrefabNameIndex, 80, (from, to)=>
			{
				GameObject proto = Resources.Load<GameObject>("Player/" + playerPrefabNames[to]);
				gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(proto));
			});
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