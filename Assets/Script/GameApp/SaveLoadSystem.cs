using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using System.IO;
using System.Text;

namespace GameApp
{
	class SaveData : IMainGameMemento
	{
		private string[] _stringValues;
		private int[] _intValues;
		private int[] _inventoryIds;

        public string[] stringValues
        {
        	get
        	{
        		return _stringValues;
        	}
        	set
        	{
        		_stringValues = value;
        	}
       	}
        public int[] intValues
        {
        	get
        	{
        		return _intValues;
        	}
        	set
        	{
        		_intValues = value;
        	}
    	}
        public int[] inventoryIds
        {
        	get
        	{
        		return _inventoryIds;
        	}
        	set
        	{
        		_inventoryIds = value;
        	}
        }
	}

	class SaveLoadSystem
	{
		private string _basePath;
		private int _capacity;

		public void Initialize(string basePath, int capacity)
		{

		}

		public SaveData LoadSaveData(int index)
		{
			string fileName = GetSaveFileName(index);
			try
			{
				FileStream fs = new FileStream(string.Format("{0}/{1}", _basePath, fileName), FileMode.Create, FileAccess.Write);
				SaveData result = new SaveData();
				DataFromStream(result, fs);
				fs.Close();
				return result;
			}
			catch(System.Exception ex)
			{
				return null;
			}
		}

		public void SaveSaveData(int index, SaveData data)
		{
			string fileName = GetSaveFileName(index);
			try
			{
				FileStream fs = new FileStream(_basePath + "/" + fileName, FileMode.Create, FileAccess.Write);
				DataToStream(data, fs);
				fs.Close();
			}
			catch(System.Exception ex)
			{
				return;
			}
		}

		public static void DataToStream(SaveData data, Stream stream)
		{

		}

		public static void DataFromStream(SaveData data, Stream stream)
		{

		}

		private static string GetSaveFileName(int index)
		{
			return string.Format("savedata_{0}", index.ToString());
		}
	}
}