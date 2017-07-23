using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKernal;

namespace MainGame
{
	interface IMenuViewListener
	{
		void OnBackButtonPressed();
	}

	class MenuView
	{
		private MonoView _monoView;
		private IMenuViewListener _listener;
		private Transform _inventoryListRoot;
		private Transform _inventoryDescRoot;

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
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnBackButtonPressed();
						});
					}

					button = _monoView.GetWidget<Button>("inventory_button");
					if (button != null)
					{
						button.onClick.AddListener(() =>
						{
							ShowInventory(!_showingInventory);
						});
					}

					_inventoryListRoot = _monoView.GetWidget<Transform>("inventory_list_root");
					_inventoryDescRoot = _monoView.GetWidget<Transform>("inventory_desc_root");
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

		private void ShowInventory(bool show)
		{
			_showingInventory = show;
			if (_inventoryListRoot != null)
				_inventoryListRoot.gameObject.SetActive(show);
			if (_inventoryDescRoot != null)
				_inventoryDescRoot.gameObject.SetActive(show);
		}
	}
}