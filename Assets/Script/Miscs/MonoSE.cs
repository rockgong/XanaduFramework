using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miscs
{
	public class MonoSE : MonoBehaviour
	{
		public float time;

		private float _timeCnt;

		private void Start()
		{
			_timeCnt = time;
		}

		private void Update()
		{
			if (_timeCnt > 0.0f)
				_timeCnt -= Time.deltaTime;
			else
				GameObject.Destroy(gameObject);
		}
	}
}