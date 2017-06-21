using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	interface IInputEventListener
	{
		void OnMouseButtonDown();
		void OnMouseButtonUp();
	}

	class MonoGameInput : MonoBehaviour
	{
		public IInputEventListener listener;

		public Vector3 GetMousePosition()
		{
			return Input.mousePosition;
		}

		private void Update()
		{
			if (listener != null)
			{
				if (Input.GetButtonDown("Fire1"))
					listener.OnMouseButtonDown();
				if (Input.GetButtonUp("Fire1"))
					listener.OnMouseButtonUp();
			}
		}
	}
}