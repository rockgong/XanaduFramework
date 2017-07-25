using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainGame;
using GameKernal;

namespace GameApp
{
	interface ISaveLoadViewListener
	{
		void OnSaveLoadButtonPressed(SaveData data);
		void OnBackButtonPressed();
	}

	class SaveLoadView
	{
		private MonoView _view;
		private ISaveLoadViewListener _listener;

		private Transform _saveDataListRoot;

		public void Initialize()
		{
			GameObject proto = Resources.Load<GameObject>("UI/SaveLoadView");
			if (proto != null)
			{
				GameObject inst = GameObject.Instantiate(proto);
				_view = inst.GetComponent<MonoView>();
				SetVisible(false);

				if (_view != null)
				{
					_saveDataListRoot = _view.GetWidget<Transform>("save_data_list_root");
				}
			}
		}

		public void SetVisible(bool visible)
		{
			if (_view != null)
				_view.gameObject.SetActive(visible);
		}

		public void SetListener(ISaveLoadViewListener listener)
		{
			_listener = listener;
		}

		public void SetupSaveDataView(SaveLoadSystem system)
		{
			if (_saveDataListRoot == null)
				return;

			foreach (Transform trans in _saveDataListRoot)
				GameObject.Destroy(trans.gameObject);

			GameObject proto = Resources.Load<GameObject>("UI/SaveDataItem");
			if (proto != null)
			{
				system.ForEachSaveData((i, data) =>
				{
					GameObject inst = GameObject.Instantiate(proto);
					inst.transform.SetParent(_saveDataListRoot);
					Button button = inst.GetComponent<Button>();
					if (button != null)
					{
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnSaveLoadButtonPressed(data);
						});
					}

					MonoView itemView = inst.GetComponent<MonoView>();
					if (itemView != null)
					{
						Text itemText = itemView.GetWidget<Text>("save_data_desc");
						if (itemText != null)
						{
							itemText.text = data == null ? "NULL" : data.ToString();
						}
					}
				});
			}
		}
	}
}