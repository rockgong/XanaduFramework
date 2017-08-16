using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miscs
{
	public class MonoFinalGrassAnim : MonoBehaviour
	{
		public GameObject[] grasses;
		public float distance;

		public Material _inDistanceMat;
		public Material _outDistanceMat;

		private class GrassInstanceItem
		{
			public float distance;
			public GameObject target;
			public bool inDistance;
		}

		private List<GrassInstanceItem> _grassInstanceItem;

		void Start()
		{
			_grassInstanceItem = new List<GrassInstanceItem>(grasses.Length);
			for (int i = 0; i < grasses.Length; i++)
			{
				_grassInstanceItem.Add(new GrassInstanceItem());
				_grassInstanceItem[i].target = grasses[i];
				_grassInstanceItem[i].distance = (grasses[i].transform.position - transform.position).magnitude;
				_grassInstanceItem[i].inDistance = false;
			}
		}

		void Update()
		{
			for (int i = 0; i < _grassInstanceItem.Count; i++)
			{
				if (distance > _grassInstanceItem[i].distance)
				{
					if (!_grassInstanceItem[i].inDistance)
					{
						Renderer[] renderers = _grassInstanceItem[i].target.GetComponentsInChildren<Renderer>();
						for (int j = 0; j < renderers.Length; j++)
							renderers[j].sharedMaterial = _inDistanceMat;
						_grassInstanceItem[i].inDistance = true;
					}
				}
				else
				{
					if (_grassInstanceItem[i].inDistance)
					{
						Renderer[] renderers = _grassInstanceItem[i].target.GetComponentsInChildren<Renderer>();
						for (int j = 0; j < renderers.Length; j++)
							renderers[j].sharedMaterial = _outDistanceMat;
						_grassInstanceItem[i].inDistance = false;
					}
				}
			}
		}
	}
}