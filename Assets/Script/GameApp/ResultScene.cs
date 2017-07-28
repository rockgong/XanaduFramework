using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace GameApp
{
	class ResultScene
	{
		private bool _initialized = false;
		private bool _startup = false;

		private MonoView _view;
		private MonoDelegate _timer;

		public void Initialize(string viewPath)
		{
			if (_initialized)
				return;

			GameObject proto = Resources.Load<GameObject>(viewPath);
			if (proto != null)
			{
				GameObject inst = GameObject.Instantiate(proto);
				if (inst != null)
				{
					_view = inst.GetComponent<MonoView>();
				}
			}

			_initialized = true;
			_startup = true;
			Shutdown();
		}

		public void Startup(float time, System.Action timeUpAction)
		{
			if (_initialized)
			{
				if (!_startup)
				{
					_view.gameObject.SetActive(true);
					float timeCnt = time;
					if (timeUpAction != null)
					{
						_timer = MonoDelegate.Create(() =>
						{
							timeCnt -= Time.deltaTime;
							if (timeCnt <= 0.0f)
							{
								timeUpAction();
								if (_timer != null)
								{
									GameObject.Destroy(_timer.gameObject);
									_timer = null;
								}
							}
						}, "_ResultViewTimer");
					}
					_startup = true;
				}
			}
		}

		public void Shutdown()
		{
			if (_initialized)
			{
				if (_startup)
				{
					if (_view != null)
					{
						_view.gameObject.SetActive(false);
						if (_timer != null)
							GameObject.Destroy(_timer.gameObject);
					}
					_startup = false;
				}
			}
		}

		public void Uninitialize()
		{
			if (!_initialized)
				return;

			if (_view != null)
				GameObject.Destroy(_view.gameObject);

			_initialized = false;
			_startup = true;
			Shutdown();
		}
	}
}