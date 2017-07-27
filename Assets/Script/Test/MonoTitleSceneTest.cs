using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameApp
{
	public class MonoTitleSceneTest : MonoBehaviour, ITitleSceneHost
	{
		public string viewPath;
		public string stagePath;

		private TitleScene _titleScene = new TitleScene();

		// Use this for initialization
		void Start ()
		{
		}

		void OnGUI()
		{
			if (GUILayout.Button("Initialize"))
			{
				_titleScene.Initialize(viewPath, stagePath, 3, this);
			}
			if (GUILayout.Button("Startup"))
			{
				_titleScene.Startup();
			}
			if (GUILayout.Button("Shutdown"))
			{
				_titleScene.Shutdown();
			}
			if (GUILayout.Button("Uninitialize"))
			{
				_titleScene.Uninitialize();
			}
		}

		public void OnSelect(int index)
		{
			Debug.LogFormat("Select_{0}", index);
		}
	}
}