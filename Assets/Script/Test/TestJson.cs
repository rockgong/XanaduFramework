using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Helper;

public class NestedConfig
{
    public int nestedIField = 0;
    public string nestedSField = string.Empty;
}

public class TestConfig
{
    public int iField = 0;
    public string sField = string.Empty;
    public int[] aField = new int[0];
    public NestedConfig nestedConfig = null;
    public NestedConfig[] nestedConfigArray = null;

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.Reflection.FieldInfo[] fields = this.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            sb.Append(fields[i].Name);
            sb.Append('=');
            sb.Append(fields[i].GetValue(this).ToString());
            sb.Append('\n');
        }
        return sb.ToString();
    }
}

public class TestJson : MonoBehaviour {
	private string _jsonFilePath = "CommonEvent/TestCommonEvent";
	private JsonData _jsonData = null;
    private TestConfig _testConfig = null;
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
            _testConfig = Helper.JsonHelper.PolymorphReflectParse(_jsonData, typeof(TestConfig)) as TestConfig;
		}
		if (_jsonData != null)
		{
			if (_testConfig != null)
            {
                GUILayout.Label(_testConfig.ToString());
            }
		}
	}
}
