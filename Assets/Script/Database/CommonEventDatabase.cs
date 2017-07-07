using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using MainGame;
using System;
using LitJson;

namespace Config
{
    public class CommonEventDatabase : ICommonEventDatabase
    {
        private class CommonEventEntry
        {
            public string name;
            public BaseCommonEvent data;
        }

        private CommonEventEntry[] _data = new CommonEventEntry[0];

        public void LoadFromAsset(string path)
        {
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
            {
                Debug.LogWarning(string.Format("path of asset not exist : {0}", path));
                return;
            }
            List<CommonEventEntry> tempList = new List<CommonEventEntry>(_data);
            CommonEventEntry[] readData = Helper.JsonHelper.PolymorphReflectParse(JsonMapper.ToObject(asset.text), typeof(CommonEventEntry[])) as CommonEventEntry[];
            for (int i = 0; i < readData.Length; i++)
            {
                BaseCommonEvent evt = GetCommonEvent(readData[i].name);
                if (evt == null)
                    tempList.Add(readData[i]);
            }
            _data = tempList.ToArray();
        }

        public void Unload()
        {
            _data = new CommonEventEntry[0];
        }

        public BaseCommonEvent GetCommonEvent(string name)
        {
            BaseCommonEvent result = null;

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i].name == name)
                {
                    result = _data[i].data;
                    break;
                }
            }

            return result;
        }
    }
}