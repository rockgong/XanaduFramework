using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	interface IInputEventListener
	{
		void OnMouseMove(Vector3 newPosition);
		void OnMouseButtonDown();
		void OnMouseButtonUp();
	}

	class MonoGameInput : MonoBehaviour
	{
		public IInputEventListener listener;

		private Vector3 _lastMousePosition = Vector3.zero;

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
				if (Input.mousePosition != _lastMousePosition)
					listener.OnMouseMove(Input.mousePosition);
			}
			_lastMousePosition = Input.mousePosition;
		}
	}
}