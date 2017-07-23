using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using LitJson;
using Helper;

namespace Config
{
	public class InventoryDatabase : IInventoryDatabase
	{
		class InventoryDatabaseEntry : IInventoryDatabaseEntry
		{
			public int ID;
			public string NAME;
			public string DESC;

			public int id{ get {return ID;} }
			public string name { get {return NAME;} }
			public string desc { get {return DESC;} }
		}

		private InventoryDatabaseEntry[] _data = null;

		public void Initialize()
		{
			TextAsset asset = Resources.Load<TextAsset>("Database/Inventory");
			JsonData jsonData = JsonMapper.ToObject(asset.text);
			_data = JsonHelper.PolymorphReflectParse(jsonData, typeof(InventoryDatabaseEntry[])) as InventoryDatabaseEntry[];
		}

		public IInventoryDatabaseEntry GetEntryById(int id)
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

        public List<IInventoryDatabaseEntry> GetEntryList()
        {
        	return new List<IInventoryDatabaseEntry>(_data);
        }
	}
}