using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameApp;
using System.Text;

namespace GameApp
{
	class MonoTestSaveLoadSystem : MonoBehaviour, ISaveLoadViewListener {
		public int stageId;
		public string stagePointName;
		public string[] stringValues;
		public int[] intValues;
		public int[] inventoryIds;
		public int saveIndex;

		private SaveLoadSystem _saveLoadSystem = new SaveLoadSystem();
		private SaveLoadView _saveLoadView = new SaveLoadView();

		private bool _showingView = false;

		void Start ()
		{
			_saveLoadSystem.Initialize(@"D:/", 10);
			_saveLoadView.Initialize();
			_saveLoadView.SetListener(this);
		}
		
		void Update ()
		{
			
		}

		void OnGUI()
		{
			if (GUILayout.Button("Save"))
			{
				SaveData saveData = new SaveData();
				saveData.stageId = stageId;
				saveData.stagePointName = stagePointName;
				saveData.stringValues = stringValues;
				saveData.intValues = intValues;
				saveData.inventoryIds = inventoryIds;

				_saveLoadSystem.SetSaveData(saveIndex, saveData);
			}
			if (GUILayout.Button("Dump"))
			{
				_saveLoadSystem.ForEachSaveData((i, data) =>
				{
					if (data == null)
						Debug.LogFormat("{0} : {1}", i.ToString(), "NULL");
					else
						Debug.LogFormat("{0} : {1}", i.ToString(), Stringify(data));
				});
			}
			if (!_showingView)
			{
				if (GUILayout.Button("Show View"))
				{
					_showingView = true;
					_saveLoadView.SetupSaveDataView(_saveLoadSystem);
					_saveLoadView.SetVisible(true);
				}
			}
			else
			{
				if (GUILayout.Button("Hide View"))
				{
					_showingView = false;
					_saveLoadView.SetVisible(false);
				}
			}
		}

		private string Stringify(SaveData saveData)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("[");
			sb.Append(saveData.stageId.ToString());
			sb.Append(",");
			sb.Append(saveData.stagePointName);
			sb.Append("]");

			sb.Append(saveData.stringValues.Length.ToString());
			sb.Append(" ");
			for (int i = 0; i < saveData.stringValues.Length; i++)
			{
				sb.Append(saveData.stringValues[i]);
				sb.Append(" ");
			}
			sb.Append("/");
			sb.Append(saveData.intValues.Length.ToString());
			sb.Append(" ");
			for (int i = 0; i < saveData.intValues.Length; i++)
			{
				sb.Append(saveData.intValues[i]);
				sb.Append(" ");
			}
			sb.Append("/");
			sb.Append(saveData.inventoryIds.Length.ToString());
			sb.Append(" ");
			for (int i = 0; i < saveData.inventoryIds.Length; i++)
			{
				sb.Append(saveData.inventoryIds[i]);
				sb.Append(" ");
			}
			return sb.ToString();
		}

		public void OnSaveLoadButtonPressed(SaveData data)
		{
			if (data != null)
				Debug.Log(data.ToString());
			else
				Debug.Log("Null Data");
		}
		public void OnBackButtonPressed()
		{
			Debug.Log("Pressed 2");
		}
	}
}