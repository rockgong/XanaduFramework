using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UnityEngine.UI;

namespace MainGame
{
	interface IInteractViewListener
	{
		void OnViewClosed();
	}

	class InteractView
	{
		private MonoView _monoView;
		private IInteractViewListener _listener;

		public void Initialize()
		{
			GameObject viewProto = Resources.Load<GameObject>("UI/InteractView");
			if (viewProto != null)
			{
				GameObject viewInst = GameObject.Instantiate(viewProto);
				_monoView = viewInst.GetComponent<MonoView>();
				if (_monoView != null)
				{
					Button bgButton = _monoView.GetWidget<Button>("bg_button");
					if (bgButton != null)
						bgButton.onClick.AddListener(() =>
						{
							CloseMessage();
							if (_listener != null)
								_listener.OnViewClosed();
						});
				}
			}
		}

		public void SetListener(IInteractViewListener listener)
		{
			_listener = listener;

			return;
		}

		public void ShowMessage(string msg)
		{
			Transform trans = _monoView.GetWidget<Transform>("message_root");
			if (trans != null)
				trans.gameObject.SetActive(true);

			Text msgText = _monoView.GetWidget<Text>("message_msg");
			if (msgText != null)
			{
				msgText.text = msg;
				RectTransform msgTrans = msgText.transform as RectTransform;
				if (msgTrans != null)
				{
					msgTrans.sizeDelta = new Vector2(msgText.preferredWidth, msgText.preferredHeight);
				}
			}

			return;
		}

		public void CloseMessage()
		{
			Transform trans = _monoView.GetWidget<Transform>("message_root");
			if (trans != null)
				trans.gameObject.SetActive(false);

			if (_listener != null)
				_listener.OnViewClosed();

			return;
		}
	}
}