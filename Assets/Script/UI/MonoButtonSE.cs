using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Audio;

namespace UIUtil
{
	public class MonoButtonSE : MonoBehaviour, IPointerClickHandler
	{
		public string name;

		public void OnPointerClick(PointerEventData data)
		{
			SoundEffect.Play(name);
		}
	}
}