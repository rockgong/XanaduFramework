using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtil
{
	public static class UIUtils
	{
		public static Vector2 WorldPointToCanvasAnchoredPosition(Vector3 position, Vector2 canvasSize)
		{
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(position);
			Vector2 screenRatio = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
			return new Vector2((screenRatio.x - 0.5f) * canvasSize.x, (screenRatio.y - 0.5f) * canvasSize.y);
		}
	}
}