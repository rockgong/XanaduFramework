using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	[System.Serializable]
	public class WidgetEntry
	{
		public string name;
		public GameObject widget;
	}

	public class MonoView : MonoBehaviour
	{
		public WidgetEntry[] widgets;

		public T GetWidget<T>(string name) where T : Component
		{
			for (int i = 0; i < widgets.Length; i++)
			{
				if (widgets[i].name == name)
				{
					T result = widgets[i].widget.GetComponent<T>();
					return result;
				}
			}

			return null;
		}
	}
}