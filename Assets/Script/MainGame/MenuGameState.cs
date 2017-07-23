using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	class MenuGameState : IGameState
	{
		private MenuView _menuView;

		public void SetMenuView(MenuView view)
		{
			_menuView = view;
		}

		public void EnterState(IGameKernal kernal)
		{
			_menuView.SetVisible(true);
		}

		public void ExitState(IGameKernal kernal)
		{
			_menuView.SetVisible(false);
		}
	}
}