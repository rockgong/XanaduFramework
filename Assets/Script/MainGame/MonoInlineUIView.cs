using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class MonoInlineUIView : MonoBehaviour
	{
		public float time;
		public System.Action endAction;

		private float _timeCnt;
		// Use this for initialization
		void Start ()
		{
			_timeCnt = time;
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (_timeCnt > 0.0f)
			{
				_timeCnt -= Time.deltaTime;
				if (_timeCnt <= 0.0f)
				{
					_timeCnt = -1.0f;
					if (endAction != null)
					{
						endAction();
						endAction = null;
					}
				}
			}
		}
	}
}