using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miscs
{
	public class MonoDummyEmit : MonoBehaviour
	{
		public MonoDummyEmit target;

		public float r;
		public float g;
		public float b;

		private Renderer _renderer;
		// Use this for initialization
		void Start () {
			_renderer = GetComponent<Renderer>();
		}
		
		// Update is called once per frame
		void Update () {
			float fr = target == null ? r : target.r;
			float fg = target == null ? g : target.g;
			float fb = target == null ? b : target.b;

			if (_renderer != null)
				_renderer.material.SetColor("_EmissionColor", new Color(fr, fg, fb));
		}
	}
}