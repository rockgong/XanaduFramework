﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using System.IO;
using System.Text;
using System;
using Config;

namespace GameApp
{
	public class SaveData : IMainGameMemento
	{
		public int stageId;
		public string stagePointName = string.Empty;
		public int playedTime = 0;

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

        public override string ToString()
        {
        	int sec = playedTime % 60;
        	int min = playedTime / 60;
        	int hour = min / 60;
			return string.Format("[{0}] {1} {2:D2}:{3:D2}:{4:D2}", TextMap.Map(string.Format("SP_{0}", stageId)), TextMap.Map("PlayTime"), hour, min, sec);
        }
	}

	public class SystemSaveData
	{
		public int completion;
	}

	public class SaveLoadSystem
	{
		private string _basePath;
		private int _capacity;
		private SaveData[] _saveData = null;
		private SystemSaveData _systemSaveData = new SystemSaveData();
		private const string SYSEM_FILE_NAME = "system_savedata";

		public void Initialize(string basePath, int capacity)
		{
			_basePath = basePath;
			_capacity = capacity;
			_saveData = new SaveData[capacity];

			for (int i = 0; i < _capacity; i++)
			{
				_saveData[i] = LoadSaveData(i);
			}

			LoadSystemSaveData();
		}

		private SaveData LoadSaveData(int index)
		{
			string fileName = GetSaveFileName(index);
			try
			{
				FileStream fs = new FileStream(string.Format("{0}/{1}", _basePath, fileName), FileMode.Open, FileAccess.Read);
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

		public SaveData GetSaveData(int index)
		{
			if (_saveData != null && _saveData.Length > index)
				return _saveData[index];
			return null;
		}

		private SaveData SaveSaveData(int index, SaveData data)
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
				return null;
			}

			return data;
		}

		public SystemSaveData GetSystemSaveData()
		{
			return _systemSaveData;
		}

		public void LoadSystemSaveData()
		{
			string filePath = _basePath + "/" + SYSEM_FILE_NAME;

			if (!File.Exists(filePath))
				SetSystemSaveData(new SystemSaveData());
			else
			{
				try
				{
					FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
					SystemSaveDataFromStream(_systemSaveData, fs);
					fs.Close();
				}
				catch(System.Exception ex)
				{
					return;
				}
			}
		}

		public SystemSaveData SetSystemSaveData(SystemSaveData data)
		{
			if (data == null)
				return null;

			_systemSaveData = data;
			string filePath = _basePath + "/" + SYSEM_FILE_NAME;
			try
			{
				FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
				SystemSaveDataToStream(_systemSaveData, fs);
				fs.Close();
			}
			catch(System.Exception ex)
			{
				return null;
			}

			return _systemSaveData;
		}

		public SaveData SetSaveData(int index, SaveData data)
		{
			if (_saveData == null || index >= _capacity)
				return null;

			SaveData savedData = SaveSaveData(index, data);
			_saveData[index] = savedData;
			return savedData;
		}

		public void ForEachSaveData(System.Action<int, SaveData> action)
		{
			for (int i = 0; i < _saveData.Length; i++)
				action(i, _saveData[i]);
		}

		public static void DataToStream(SaveData data, Stream stream)
		{
			//stageId
			byte[] buf = BitConverter.GetBytes(data.stageId);
			stream.Write(buf, 0, buf.Length);

			//stagePointName
			buf = System.Text.Encoding.Unicode.GetBytes(data.stagePointName);
			byte[] tempBuf = BitConverter.GetBytes(buf.Length);
			stream.Write(tempBuf, 0, tempBuf.Length);
			stream.Write(buf, 0, buf.Length);

			//playedTime
			buf = BitConverter.GetBytes(data.playedTime);
			stream.Write(buf, 0, buf.Length);

			//string count
			buf = BitConverter.GetBytes(data.stringValues.Length);
			stream.Write(buf, 0, buf.Length);

			//string content
			for (int i = 0; i < data.stringValues.Length; i++)
			{
				buf = System.Text.Encoding.Unicode.GetBytes(data.stringValues[i]);
				byte[] innerBuf = BitConverter.GetBytes(buf.Length);
				stream.Write(innerBuf, 0 , innerBuf.Length);
				stream.Write(buf, 0, buf.Length);
			}

			//int count
			buf = BitConverter.GetBytes(data.intValues.Length);
			stream.Write(buf, 0, buf.Length);

			//int content
			for (int i = 0; i < data.intValues.Length; i++)
			{
				buf = BitConverter.GetBytes(data.intValues[i]);
				stream.Write(buf, 0, buf.Length);
			}

			//inventory count
			buf = BitConverter.GetBytes(data.inventoryIds.Length);
			stream.Write(buf, 0, buf.Length);

			//inventory content
			for (int i = 0; i < data.inventoryIds.Length; i++)
			{
				buf = BitConverter.GetBytes(data.inventoryIds[i]);
				stream.Write(buf, 0, buf.Length);
			}
		}

		public static void DataFromStream(SaveData data, Stream stream)
		{
			//stageId
			byte[] buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			int intVal = BitConverter.ToInt32(buf, 0);
			data.stageId = intVal;

			//stagePointName
			buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			int tempLength = BitConverter.ToInt32(buf, 0);
			buf = new byte[tempLength];
			stream.Read(buf, 0, buf.Length);
			data.stagePointName = System.Text.Encoding.Unicode.GetString(buf);

			//playedTime
			buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			intVal = BitConverter.ToInt32(buf, 0);
			data.playedTime = intVal;

			//string count
			buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			intVal = BitConverter.ToInt32(buf, 0);
			data.stringValues = new string[intVal];

			//string content
			for (int i = 0; i < data.stringValues.Length; i++)
			{
				byte[] innerBuf = new byte[4];
				stream.Read(innerBuf, 0, innerBuf.Length);
				int contentLength = BitConverter.ToInt32(innerBuf, 0);
				buf = new byte[contentLength];
				stream.Read(buf, 0, buf.Length);
				data.stringValues[i] = System.Text.Encoding.Unicode.GetString(buf);
			}

			//int count
			buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			intVal = BitConverter.ToInt32(buf, 0);
			data.intValues = new int[intVal];

			//int content
			for (int i = 0; i < data.intValues.Length; i++)
			{
				buf = new byte[4];
				stream.Read(buf, 0, buf.Length);
				intVal = BitConverter.ToInt32(buf, 0);
				data.intValues[i] = intVal;
			}

			//inventory count
			buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			intVal = BitConverter.ToInt32(buf, 0);
			data.inventoryIds = new int[intVal];

			//inventory content
			for (int i = 0; i < data.inventoryIds.Length; i++)
			{
				buf = new byte[4];
				stream.Read(buf, 0, buf.Length);
				intVal = BitConverter.ToInt32(buf, 0);
				data.inventoryIds[i] = intVal;
			}
		}

		public static void SystemSaveDataFromStream(SystemSaveData data, Stream stream)
		{
			// completion
			byte[] buf = new byte[4];
			stream.Read(buf, 0, buf.Length);
			int intVal = BitConverter.ToInt32(buf, 0);
			data.completion = intVal;
		}

		public static void SystemSaveDataToStream(SystemSaveData data, Stream stream)
		{
			// completion
			byte[] buf = BitConverter.GetBytes(data.completion);
			stream.Write(buf, 0, buf.Length);
		}

		private static string GetSaveFileName(int index)
		{
			return string.Format("savedata_{0}", index.ToString());
		}
	}
}