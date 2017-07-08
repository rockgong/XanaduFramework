using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Helper;
using MainGame;

namespace Config
{
	public class PropObjectDatabase : IPropObjectDatabase {
		class PropObjectDatabaseEntry : IPropObjectDatabaseEntry
		{
			public int ID;
			public string NAME;
			public string PREFAB_NAME;

			public int id {get{return ID;}}
			public string name{get{return NAME;}}
			public string prefabName{get{return PREFAB_NAME;}}
		}

		private PropObjectDatabaseEntry[] _data = null;
		
		public void Initialize()
		{
			TextAsset asset = Resources.Load<TextAsset>("Database/PropObject");
			JsonData jsonData = JsonMapper.ToObject(asset.text);
			_data = JsonHelper.PolymorphReflectParse(jsonData, typeof(PropObjectDatabaseEntry[])) as PropObjectDatabaseEntry[];
		}

        public IPropObjectDatabaseEntry GetEntryById(int id)
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
        public List<IPropObjectDatabaseEntry> GetEntryList()
        {
        	return new List<IPropObjectDatabaseEntry>(_data);
        }
	}
}