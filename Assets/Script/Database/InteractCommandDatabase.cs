using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using System;
using Helper;
using LitJson;

namespace Config
{
    public class InteractCommandDatabase : IInteractCommandDatabase
    {
        private class InteractCommandEntry
        {
            public int id;
            public BaseInteractCommandData data;
        }

        private InteractCommandEntry[] _data = new InteractCommandEntry[0];

        public void LoadFromAsset(string path)
        {
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
            {
                Debug.LogWarning(string.Format("path of asset not exist : {0}", path));
                return;
            }
            List<InteractCommandEntry> tempList = new List<InteractCommandEntry>(_data);
            InteractCommandEntry[] readData = Helper.JsonHelper.PolymorphReflectParse(JsonMapper.ToObject(asset.text), typeof(InteractCommandEntry[])) as InteractCommandEntry[];
            for (int i = 0; i < readData.Length; i++)
            {
                BaseInteractCommandData evt = GetDataById(readData[i].id);
                if (evt == null)
                    tempList.Add(readData[i]);
            }
            _data = tempList.ToArray();
        }

        public BaseInteractCommandData GetDataById(int id)
        {
            BaseInteractCommandData result = null;

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