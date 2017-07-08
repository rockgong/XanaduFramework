using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public static class TextMap
    {
        private static Dictionary<string, string> _dict = null;

        public static void Initialize()
        {
            _dict = new Dictionary<string, string>();
            TextAsset asset = Resources.Load<TextAsset>("TextMap/TextMap");
            string[] lines = asset.text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                int indexOfTab = lines[i].IndexOf('\t');
                if (indexOfTab != -1)
                {
                    _dict[lines[i].Substring(0, indexOfTab + 1)] = lines[i].Substring(indexOfTab + 1);
                }
            }
        }

        public static string Map(string key)
        {
            if (_dict != null && _dict.ContainsKey(key))
                return _dict[key];
            return string.Format("[{0}]", key);
        }
    }
}