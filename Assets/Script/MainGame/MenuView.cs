using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKernal;

namespace MainGame
{
	class IMenuViewListener
	{

	}

	class MenuView
	{
		private MonoView _monoView;
		private IMenuViewListener _listener;

		public void Initialize()
		{
			GameObject viewProto = Resources.Load<GameObject>("UI/MenuView");
			if (viewProto)
			{
				GameObject viewInst = GameObject.Instantiate(viewProto);
				_monoView = viewInst.GetComponent<MonoView>();
				if (_monoView != null)
				{
					//TODO:
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
	}
}