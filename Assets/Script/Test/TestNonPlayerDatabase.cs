using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    [System.Serializable]
    public class TestNonPlayerDatabaseEntry : INonPlayerDatabaseEntry
    {
        public int idConfig;
        public string nameConfig;
        public string prefabNameConfig;

        public int id
        {
            get
            {
                return idConfig;
            }
        }

        public string name
        {
            get
            {
                return nameConfig;
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

    public class TestNonPlayerDatabase : MonoBehaviour, INonPlayerDatabase
    {
        public TestNonPlayerDatabaseEntry[] entries;

        public INonPlayerDatabaseEntry GetEntryById(int id)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].idConfig == id)
                    return entries[i];
            }

            return null;
        }

        public List<INonPlayerDatabaseEntry> GetEntryList()
        {
            return new List<INonPlayerDatabaseEntry>(entries);
        }
    }
}