using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Helper;
using MainGame;

namespace Config
{
	public class StageDatabase : IStageDatabase
	{
		class StageDatabaseEntry : IStageDatabaseEntry
		{
			public int ID;
			public string SCENE_NAME;
			public BaseCommonVector3 CAMERA_LOOK;
			public BaseCommonVector3 CAMERA_POS;
			public StageTransferData[] TRANSFERS;

			public int id {get{return ID;}}
			public string sceneName{get{return SCENE_NAME;}}
			public BaseCommonVector3 cameraLook{get{return CAMERA_LOOK;}}
			public BaseCommonVector3 cameraPos{get{return CAMERA_POS;}}
			public StageTransferData[] transfers{get{return TRANSFERS;}}
		}

		private StageDatabaseEntry[] _data = null;
		
		public void Initialize()
		{
			TextAsset asset = Resources.Load<TextAsset>("Database/Stage");
			JsonData jsonData = JsonMapper.ToObject(asset.text);
			_data = JsonHelper.PolymorphReflectParse(jsonData, typeof(StageDatabaseEntry[])) as StageDatabaseEntry[];
		}

        public IStageDatabaseEntry GetEntryById(int id)
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
        public List<IStageDatabaseEntry> GetEntryList()
        {
        	return new List<IStageDatabaseEntry>(_data);
        }
    }
}