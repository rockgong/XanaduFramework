using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class MainGameState : IGameState
	{
		public void EnterState(IGameKernal kernal)
		{
			Debug.Log("Enter Main Game State");

			return;
		}

		public void ExitState(IGameKernal kernal)
		{
			Debug.Log("Exit Main Game State");

			return;
		}
	}
}