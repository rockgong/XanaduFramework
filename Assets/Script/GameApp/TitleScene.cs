using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKernal;

namespace GameApp
{
	interface ITitleSceneHost
	{
		void OnSelect(int index);
	}

	class TitleScene
	{
		private MonoView _view;
		private MonoStage _stage;
		private ITitleSceneHost _host;

		private bool _initiliazed = false;
		private bool _startup = false;

		public void Initialize(string viewPath, string stagePath, int maxSelectIndex, ITitleSceneHost host)
		{
			if (_initiliazed || string.IsNullOrEmpty(viewPath)|| string.IsNullOrEmpty(stagePath))
				return;

			GameObject proto = null;
			GameObject inst = null;
			proto = Resources.Load<GameObject>(viewPath);
			if (proto != null)
			{
				inst = GameObject.Instantiate(proto);
				_view = inst.GetComponent<MonoView>();
				if (_view != null)
				{
					for (int i = 0; i < maxSelectIndex; i++)
					{
						Button button = _view.GetWidget<Button>(string.Format("select_{0}", i));
						if (button != null)
						{
							int index = i;
							button.onClick.RemoveAllListeners();
							button.onClick.AddListener(() =>
							{
								if (_host != null)
									_host.OnSelect(index);
							});
						}
					}
					_view.gameObject.SetActive(false);
				}
			}
			else
				Debug.Log("View Path incorrect");

			proto = Resources.Load<GameObject>(stagePath);
			if (proto != null)
			{
				inst = GameObject.Instantiate(proto);
				_stage = inst.GetComponent<MonoStage>();
				if (_stage != null)
				{
					Transform trans = _stage.GetPointTrans("camera_pos");
					if (trans != null)
					{
						Camera.main.transform.position = trans.position;
					}
					Camera.main.transform.LookAt(_stage.transform.position);
					_stage.gameObject.SetActive(false);
				}
			}
			else
				Debug.Log("Stage Path incorrect");

			_initiliazed = true;
			_host = host;
		}

		public void Uninitialize()
		{
			if (_initiliazed)
			{
				if (_view != null)
					GameObject.Destroy(_view.gameObject);

				if (_stage != null)
					GameObject.Destroy(_stage.gameObject);

				_host = null;
				_initiliazed = false;
			}
		}

		public void Startup()
		{
			if (!_initiliazed || _startup)
				return;

			if (_view != null)
			{
				_view.gameObject.SetActive(true);
			}

			if (_stage != null)
			{
				_stage.gameObject.SetActive(true);
				Transform trans = _stage.GetPointTrans("camera_pos");
				if (trans != null)
				{
					Camera.main.transform.position = trans.position;
				}
				Camera.main.transform.LookAt(_stage.transform.position);
			}

			_startup = true;
		}

		public void Shutdown()
		{
			if (_initiliazed && _startup)
			{
				if (_view != null)
				{
					_view.gameObject.SetActive(false);
				}

				if (_stage != null)
				{
					_stage.gameObject.SetActive(false);
				}
				_startup = false;
			}
		}
	}
}