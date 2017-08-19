using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	public static class SoundEffect
	{
		private const string BASE_PATH = "Audio/SE";

		public static void Play(string name)
		{
			GameObject proto = Resources.Load<GameObject>(string.Format("{0}/{1}", BASE_PATH, name));
			if (proto == null)
			{
				Debug.LogWarning(string.Format("SE : {0} not exist", name));
				return;
			}

			GameObject inst = GameObject.Instantiate(proto);
		}
	}
}