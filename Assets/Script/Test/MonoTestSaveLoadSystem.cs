using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameApp;
using System.Text;

public class MonoTestSaveLoadSystem : MonoBehaviour {
	public string[] stringValues;
	public int[] intValues;
	public int[] inventoryIds;
	public int saveIndex;

	private SaveLoadSystem _saveLoadSystem = new SaveLoadSystem();

	void Start ()
	{
		_saveLoadSystem.Initialize(@"D:/", 10);
	}
	
	void Update ()
	{
		
	}

	void OnGUI()
	{
		if (GUILayout.Button("Save"))
		{
			SaveData saveData = new SaveData();
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
	}

	private string Stringify(SaveData saveData)
	{
		StringBuilder sb = new StringBuilder();
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
}
