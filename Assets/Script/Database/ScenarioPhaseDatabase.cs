using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using MainGame;
using System;
using LitJson;

namespace Config
{
	public class ScenarioPhaseDatabase : IScenarioPhaseDatabase
	{
		private class ScenarioPhaseEntry
		{
			public int id;
			public BaseScenarioPhaseData data;
		}

		private ScenarioPhaseEntry[] _data = new ScenarioPhaseEntry[0];

		public void LoadFromAsset(string path)
        {
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
            {
                Debug.LogWarning(string.Format("path of asset not exist : {0}", path));
                return;
            }
            List<ScenarioPhaseEntry> tempList = new List<ScenarioPhaseEntry>(_data);
            ScenarioPhaseEntry[] readData = Helper.JsonHelper.PolymorphReflectParse(JsonMapper.ToObject(asset.text), typeof(ScenarioPhaseEntry[])) as ScenarioPhaseEntry[];
            for (int i = 0; i < readData.Length; i++)
            {
                BaseScenarioPhaseData evt = GetDataById(readData[i].id);
                if (evt == null)
                    tempList.Add(readData[i]);
            }
            _data = tempList.ToArray();
        }

        public BaseScenarioPhaseData GetDataById(int id)
        {
            BaseScenarioPhaseData result = null;

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i].id == id)
                {
                    result = _data[i].data;
                    break;
                }
            }

            return result;
        }
	}
}