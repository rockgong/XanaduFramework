using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	public class MonoAnimation : MonoBehaviour
	{
		private Animation _animation;
		private System.Action _action;
		// Use this for initialization
		void Awake ()
		{
			_animation = GetComponent<Animation>();

			return;
		}
		
		// Update is called once per frame
		void Update () {
			if (_animation != null && !_animation.isPlaying)
			{
				if (_action != null)
				{
					_action();
					_action = null;
				}
			}
		}

		public void Play(string name, System.Action onEnd)
		{
			if (_animation != null && !_animation.isPlaying)
			{
                foreach (AnimationState state in _animation)
                {
                    Debug.Log(state.name);
                }
				_action = onEnd;
				_animation.Play(name);
			}
		}
	}
}