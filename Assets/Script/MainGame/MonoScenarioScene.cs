using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	[System.Serializable]
	public class ScenarioSceneNode
	{
		public string name;
		public Transform trans;
	}

	public class MonoScenarioScene : MonoBehaviour
	{
		public Transform cameraRoot;
		public ScenarioSceneNode[] nodes;

		public Transform FindNode(string name)
		{
			if (nodes == null)
				return null;

			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i].name == name)
					return nodes[i].trans;
			}

			return null;
		}
	}
}