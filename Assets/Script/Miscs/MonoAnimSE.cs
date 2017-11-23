using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Miscs
{
	public class MonoAnimSE : MonoBehaviour
	{
		public void PlaySE(string name)
		{
			if (!string.IsNullOrEmpty(name))
				SoundEffect.Play(name);
		}
	}
}