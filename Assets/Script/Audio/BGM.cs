using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	public static class BGM
	{
		private const string BASE_PATH = "Audio/BGM";
		private static string _currentBGMName = string.Empty;
		private static AudioSource _currentSource = null;

		public static void Play(string name)
		{
			if (_currentBGMName == name)
				return;

			GameObject proto = Resources.Load<GameObject>(string.Format("{0}/{1}", BASE_PATH, name));
			if (proto == null)
			{
				Debug.LogWarning(string.Format("BGM : {0} missing resource", name));
				return;
			}

			GameObject inst = GameObject.Instantiate(proto);
			AudioSource src = inst.GetComponent<AudioSource>();
			if (src == null)
			{
				GameObject.Destroy(inst);
				return;
			}

			Stop();

			_currentBGMName = name;
			_currentSource = src;
		}

		public static void Stop()
		{
			if (_currentSource == null)
				return;

			_currentSource.Stop();
			GameObject.Destroy(_currentSource.gameObject);
			_currentSource = null;
			_currentBGMName = string.Empty;

			return;
		}
	}
}