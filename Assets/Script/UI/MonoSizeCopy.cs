using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtil
{
	public class MonoSizeCopy : MonoBehaviour
	{
		public RectTransform reference;
		
		// Update is called once per frame
		void Update () {
			if (reference == null)
				return;

			RectTransform selfRect = transform as RectTransform;
			if (selfRect != null)
			{
				selfRect.anchoredPosition = reference.anchoredPosition;
				selfRect.sizeDelta = new Vector2(reference.rect.width, reference.rect.height);
			}
		}
	}
}