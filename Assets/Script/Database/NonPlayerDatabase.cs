using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Helper;
using MainGame;

namespace Config
{
	public class NonPlayerDatabase : INonPlayerDatabase {
		class NonPlayerDatabaseEntry : INonPlayerDatabaseEntry
		{
			public int ID;
			public string NAME;
			public string PREFAB_NAME;

			public int id {get{return ID;}}
			public string name{get{return NAME;}}
			public string prefabName{get{return PREFAB_NAME;}}
		}

		private NonPlayerDatabaseEntry[] _data = null;
		
		public void Initialize()
		{
			TextAsset asset = Resources.Load<TextAsset>("Database/NonPlayer");
			JsonData jsonData = JsonMapper.ToObject(asset.text);
			_data = JsonHelper.PolymorphReflectParse(jsonData, typeof(NonPlayerDatabaseEntry[])) as NonPlayerDatabaseEntry[];
		}

        public INonPlayerDatabaseEntry GetEntryById(int id)
        {
        	if (_data != null)
        	{
        		for (int i = 0; i < _data.Length; i++)
        		{
        			if (id == _data[i].id)
        				return _data[i];
        		}
        	}

        	return null;
        }
        public List<INonPlayerDatabaseEntry> GetEntryList()
        {
        	return new List<INonPlayerDatabaseEntry>(_data);
        }
	}
}