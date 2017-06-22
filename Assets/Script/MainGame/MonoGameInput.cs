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
		void OnMouseButtonRightDown();
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
			if (Input.GetButtonDown("Fire1"))
				if (listener != null) listener.OnMouseButtonDown();
			if (Input.GetButtonUp("Fire1"))
				if (listener != null) listener.OnMouseButtonUp();
			if (Input.GetButtonDown("Fire2"))
				if (listener != null) listener.OnMouseButtonRightDown();
			if (Input.mousePosition != _lastMousePosition)
				if (listener != null) listener.OnMouseMove(Input.mousePosition);
			_lastMousePosition = Input.mousePosition;
		}
	}
}