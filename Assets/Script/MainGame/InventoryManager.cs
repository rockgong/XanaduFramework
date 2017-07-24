using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public interface IInventoryDatabaseEntry
	{
		int id{ get; }
		string name { get; }
		string desc { get; }
	}

	public interface IInventoryDatabase
	{
		IInventoryDatabaseEntry GetEntryById(int id);
		List<IInventoryDatabaseEntry> GetEntryList();
	}

	public class InventoryInfo
	{
		public IInventoryDatabaseEntry data;
	}

	interface IInventoryManagerListener
	{
		void OnInventoryAdded(InventoryInfo info);
		void OnInventoryRemoved(InventoryInfo info);
	}

	class InventoryManager
	{
		private List<InventoryInfo> _inventoryInfoList = new List<InventoryInfo>();
		private List<IInventoryManagerListener> _listeners = new List<IInventoryManagerListener>();
		private IInventoryDatabase _database = null;
		private int _maxInventoryCount = 8;

		public void Initialize(IInventoryDatabase db, int maxCount)
		{
			_database = db;
			_maxInventoryCount = maxCount;
		}

        public void RegisterListener(IInventoryManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    return;
            }

            _listeners.Add(listener);
        }

        public void UnregisterListener(IInventoryManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    _listeners.Remove(listener);
            }
        }

        public void ClearListener()
        {
            _listeners.Clear();
        }

		public InventoryInfo AddInventory(int id)
		{
			if (_inventoryInfoList.Count >= _maxInventoryCount)
				return null;

			IInventoryDatabaseEntry entry = _database.GetEntryById(id);
			if (entry != null)
			{
				InventoryInfo info = new InventoryInfo();
				info.data = entry;
				_inventoryInfoList.Add(info);
				for (int i = 0; i < _listeners.Count; i++)
					_listeners[i].OnInventoryAdded(info);
				return info;
			}

			return null;
		}

		public InventoryInfo GetInventoryById(int id)
		{
			for (int i = 0; i < _inventoryInfoList.Count; i++)
			{
				if (_inventoryInfoList[i].data.id == id)
					return _inventoryInfoList[i];
			}

			return null;
		}

		public int GetInventoryCount()
		{
			return _inventoryInfoList.Count;
		}

		public void RemoveInventory(InventoryInfo info)
		{
			if (_inventoryInfoList.Contains(info))
			{
				_inventoryInfoList.Remove(info);
				for (int i = 0; i < _listeners.Count; i++)
					_listeners[i].OnInventoryRemoved(info);
			}
		}

		public void ForEachInventory(System.Action<InventoryInfo> cb)
		{
			if (cb == null)
				return;
			for (int i = 0; i < _inventoryInfoList.Count; i++)
			{
				cb(_inventoryInfoList[i]);
			}
		}

		public void Uninitialize()
		{
			_inventoryInfoList.Clear();
		}

		public void ClearInventory()
		{
			_inventoryInfoList.Clear();
		}
	}
}