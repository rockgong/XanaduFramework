using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    [System.Serializable]
    public class TestStageDatabaseEntry : IStageDatabaseEntry
    {
        public int idConfig;
        public string prefabNameConfig;

        public int id
        {
            get
            {
                return idConfig;
            }
        }

        public string prefabName
        {
            get
            {
                return prefabNameConfig;
            }
        }
    }

    public class TestStageDatabase : MonoBehaviour, IStageDatabase
    {
        public TestStageDatabaseEntry[] entries;

        public IStageDatabaseEntry GetEntryById(int id)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].id == id)
                    return entries[i];
            }

            return null;
        }
    }
}