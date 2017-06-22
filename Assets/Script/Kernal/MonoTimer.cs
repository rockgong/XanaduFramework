using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	public class MonoTimer : MonoBehaviour
	{
		private float _time;
		private float _timeCnt;
		private System.Action _action;

		public void Setup(float time, System.Action action)
		{
			_time = time;
			_timeCnt = 0.0f;
			_action = action;
		}

		void Update ()
		{
			if (_action != null)
			{
				_timeCnt += Time.deltaTime;
				if (_timeCnt > _time)
				{
					_action();
					GameObject.Destroy(gameObject);
				}
			}
			else
				GameObject.Destroy(gameObject);
		}
	}
}