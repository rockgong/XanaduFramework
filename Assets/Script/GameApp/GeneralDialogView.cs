using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKernal;

namespace GameApp
{
	class GeneralDialogView
	{
		public enum GeneralDialogMode
		{
			SingleButton,
			DoubleButton
		}

		private MonoView _view;
		private Text _contentText;
		private Button _okButton;
		private Button _cancelButton;

		public void Initialize()
		{
			GameObject proto = Resources.Load<GameObject>("UI/GeneralDialogView");
			if (proto != null)
			{
				GameObject inst = GameObject.Instantiate(proto);
				_view = inst.GetComponent<MonoView>();
			if (_view != null)
				_view.gameObject.SetActive(false);

				if (_view != null)
				{
					_contentText = _view.GetWidget<Text>("content");
					_okButton = _view.GetWidget<Button>("ok_button");
					_cancelButton = _view.GetWidget<Button>("cancel_button");
				}
			}
		}

		public void Open(GeneralDialogMode mode, string content, System.Action okAction, System.Action cancelAction = null)
		{
			if (_view != null)
				_view.gameObject.SetActive(true);

			if (_contentText != null)
				_contentText.text = content;

			if (_okButton != null)
			{
				_okButton.onClick.RemoveAllListeners();
				_okButton.onClick.AddListener(CreateButtonCallback(okAction));
			}

			bool needCancelButton = mode == GeneralDialogMode.DoubleButton;
			if (_cancelButton != null)
			{
				_cancelButton.gameObject.SetActive(needCancelButton);
				if (needCancelButton)
				{
					_cancelButton.onClick.RemoveAllListeners();
					_cancelButton.onClick.AddListener(CreateButtonCallback(cancelAction));
				}
			}

			return;
		}

		private	UnityEngine.Events.UnityAction CreateButtonCallback(System.Action action)
		{
			return () =>
			{
				if (_view != null)
					_view.gameObject.SetActive(false);
				if (action != null)
					action();
			};
		}
	}
}