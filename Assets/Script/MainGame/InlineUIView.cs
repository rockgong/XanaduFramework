using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	class InlineUIView
	{
		private MonoInlineUIView _view;
		private System.Action _endAction;
		private string _basePath;

		public void Initialize(string basePath)
		{
			_basePath = basePath;
		}

		public void ShowUI(string name, System.Action endAction)
		{
			if (_view != null)
				return;

			GameObject proto = Resources.Load<GameObject>(_basePath + "/" + name);
			if (proto != null)
			{
				GameObject inst = GameObject.Instantiate(proto);
				_view = inst.GetComponent<MonoInlineUIView>();
				if (_view != null)
					_view.endAction = () => CloseUI(false);
				_endAction = endAction;
			}
		}

		public void CloseUI(bool ignoreAction = false)
		{
			if (_view == null)
				return;

			GameObject.Destroy(_view.gameObject);
			_view.endAction = null;
			_view = null;
			if (!ignoreAction && _endAction != null)
			{
				_endAction();
				_endAction = null;
			}
		}

		public void Uninitialize()
		{
			if (_view != null)
				CloseUI();
		}
	}
}