using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UnityEngine.UI;

namespace MainGame
{
	interface IMainGameViewListener
	{
		void OnMenuButtonPressed();
	}

	class MainGameView
	{
		private MonoView _monoView;
		private IMainGameViewListener _listener;

		public void Initialize()
		{
			GameObject viewProto = Resources.Load<GameObject>("UI/MainGameView");
			if (viewProto != null)
			{
				GameObject viewInst = GameObject.Instantiate(viewProto);
				_monoView = viewInst.GetComponent<MonoView>();
				if (_monoView != null)
				{
					Button entryButton = _monoView.GetWidget<Button>("menu_entry");
					if (entryButton)
					{
						entryButton.onClick.AddListener(() =>
						{
							if (_listener != null)
								_listener.OnMenuButtonPressed();
						});
					}
				}
			}
		}

		public void SetVisible(bool visible)
		{
			if (_monoView != null)
				_monoView.gameObject.SetActive(visible);
		}

		public void SetListener(IMainGameViewListener listener)
		{
			_listener = listener;
		}
	}
}