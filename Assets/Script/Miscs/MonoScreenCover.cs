using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Miscs
{
	public class MonoScreenCover : MonoBehaviour
	{
		public Image targetImage;
		public float transSpeed;
		public float targetAlpha;
		public bool transfering;
		public System.Action action;

		private void Update()
		{
			if (targetImage == null)
				return;

			float maxAlpha = targetImage.color.a + transSpeed * Time.deltaTime;
			float minAlpha = targetImage.color.a - transSpeed * Time.deltaTime;

			maxAlpha = Mathf.Min(maxAlpha, 1.0f);
			minAlpha = Mathf.Max(minAlpha, 0.0f);

			float finalAlpha = 0.0f;
			if (targetAlpha >= minAlpha && targetAlpha <= maxAlpha)
			{
				finalAlpha = targetAlpha;
				if (transfering)
				{
					transfering = false;
					if (action != null)
					{
						action();
						action = null;
					}
				}
			}
			else
			{
				finalAlpha = Mathf.Clamp(targetAlpha, minAlpha, maxAlpha);
				transfering = true;
			}

			targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, finalAlpha);
		}
	}
}