using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainGame;
using GameKernal;
using Config;

namespace GameApp
{
	interface ISaveLoadViewListener
	{
		void OnSaveLoadButtonPressed(int index, SaveData data);
		void OnBackButtonPressed();
	}

	class SaveLoadView
	{
		public enum SaveLoadMode
		{
			Save,
			Load
		}

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
					Button backButton = _view.GetWidget<Button>("back_button");
					if (backButton != null)
					{
						backButton.onClick.RemoveAllListeners();
						backButton.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnBackButtonPressed();
						});
					}
				}
			}
		}

		public void SetVisible(bool visible)
		{
			if (_view != null)
				_view.gameObject.SetActive(visible);
		}

		public void SwitchLabel(SaveLoadMode mode)
		{
			if (_view == null)
				return;

			Transform trans = null;
			trans = _view.GetWidget<Transform>("save_label");
			if (trans != null)
				trans.gameObject.SetActive(mode == SaveLoadMode.Save);
			trans = _view.GetWidget<Transform>("load_label");
			if (trans != null)
				trans.gameObject.SetActive(mode == SaveLoadMode.Load);
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
						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnSaveLoadButtonPressed(i, data);
						});
					}

					MonoView itemView = inst.GetComponent<MonoView>();
					if (itemView != null)
					{
						Text itemText = itemView.GetWidget<Text>("save_data_desc");
						if (itemText != null)
						{
							itemText.text = data == null ? TextMap.Map("1005") : data.ToString();
						}
					}
				});
			}
		}

		public void UpdateSingleSaveData(int index, SaveData saveData)
		{	
			if (_saveDataListRoot == null)
				return;

			Transform trans = _saveDataListRoot.GetChild(index);

			if (trans != null)
			{
				Button button = trans.GetComponent<Button>();
				if (button != null)
				{
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(() =>
					{
						if (_listener != null)
							_listener.OnSaveLoadButtonPressed(index, saveData);
					});
				}

				MonoView itemView = trans.GetComponent<MonoView>();
				if (itemView != null)
				{
					Text itemText = itemView.GetWidget<Text>("save_data_desc");
					if (itemText != null)
					{
						itemText.text = saveData == null ? TextMap.Map("1005") : saveData.ToString();
					}
				}
			}
		}
	}
}