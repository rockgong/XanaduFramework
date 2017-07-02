using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class TestJson : MonoBehaviour {
	private string _jsonFilePath = "CommonEvent/TestCommonEvent";
	private JsonData _jsonData = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		_jsonFilePath = GUILayout.TextField(_jsonFilePath);
		if (GUILayout.Button("GO"))
		{
			TextAsset asset = Resources.Load<TextAsset>(_jsonFilePath);
			_jsonData = JsonMapper.ToObject(asset.text);
		}
		if (_jsonData != null)
		{
			GUILayout.Label(_jsonData.ToJson());
		}
	}
}
