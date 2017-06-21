using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractGameState : IGameState
	{
		public void EnterState(IGameKernal kernal)
		{
			Debug.Log("Enter InteractGameState");
		}

		public void ExitState(IGameKernal kernal)
		{
			Debug.Log("Exit InteractGameState");
		}
	}
}