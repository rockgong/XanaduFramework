using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKernal;

namespace MainGame
{
	interface IMenuViewListener
	{
		void OnInventoryButtonPressed(InventoryInfo info);
		void OnBackButtonPressed();
		void OnBackToMainButtonPressed();
	}

	class MenuView
	{
		private MonoView _monoView;
		private IMenuViewListener _listener;
		private Transform _inventoryListRoot;

		private bool _showingInventory;

		public void Initialize()
		{
			GameObject viewProto = Resources.Load<GameObject>("UI/MenuView");
			if (viewProto)
			{
				GameObject viewInst = GameObject.Instantiate(viewProto);
				_monoView = viewInst.GetComponent<MonoView>();
				if (_monoView != null)
				{
					Button button = _monoView.GetWidget<Button>("back_button");
					if (button != null)
					{
						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnBackButtonPressed();
						});
					}

					button = _monoView.GetWidget<Button>("inventory_button");
					if (button != null)
					{
						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() =>
						{
							ShowInventory(!_showingInventory);
						});
					}

					button = _monoView.GetWidget<Button>("back_to_main_button");
					if (button != null)
					{
						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnBackToMainButtonPressed();
						});
					}

					_inventoryListRoot = _monoView.GetWidget<Transform>("inventory_list_root");
					ShowInventory(false);
				}
			}
		}

		public void SetVisible(bool visible)
		{
			if (_monoView != null)
				_monoView.gameObject.SetActive(visible);
		}

		public void SetListener(IMenuViewListener listener)
		{
			_listener = listener;
		}

		public void SetupInventoryList(InventoryManager mgr)
		{
			foreach (Transform trans in _inventoryListRoot)
				GameObject.Destroy(trans.gameObject);
			GameObject proto = Resources.Load<GameObject>("UI/InventoryButton");
			if (proto != null)
			{
				mgr.ForEachInventory((info) =>
				{
					GameObject inst = GameObject.Instantiate(proto);
					inst.transform.SetParent(_inventoryListRoot);
					Button button = inst.GetComponent<Button>();
					if (button != null)
					{
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnInventoryButtonPressed(info);
						});
					}

					MonoView itemView = inst.GetComponent<MonoView>();
					if (itemView != null)
					{
						Text itemText = itemView.GetWidget<Text>("item_name");
						if (itemText != null)
						{
							itemText.text = info.data.name;
						}
					}
				});
			}
		}

		public void ShowInventory(bool show)
		{
			_showingInventory = show;
			if (_inventoryListRoot != null)
				_inventoryListRoot.gameObject.SetActive(show);
		}
	}
}