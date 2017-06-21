using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
	public class MonoGameKernal : MonoBehaviour
	{
		public System.Action<float> updateAction;
		
		// Update is called once per frame
		void Update () {
			if (updateAction != null)
				updateAction(Time.deltaTime);

			return;
		}
	}
}