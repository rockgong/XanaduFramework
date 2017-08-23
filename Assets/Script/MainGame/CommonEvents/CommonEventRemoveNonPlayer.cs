using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonEventRemoveNonPlayer : BaseCommonEvent
	{
		public string nonPlayerName;
	}

	class MainGameCommandRemoveNonPlayer : BaseMainGameCommand
	{
		public string nonPlayerName;

		public override void Excute(MainGameCommandManager mgr)
		{
			IGameKernal kernal = mgr.gameKernal;
			if (kernal != null)
				kernal.RemoveNonPlayerCharacter(nonPlayerName);
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventRemoveNonPlayer targetEvt = (CommonEventRemoveNonPlayer)evt;

			MainGameCommandRemoveNonPlayer result = new MainGameCommandRemoveNonPlayer();
			result.nonPlayerName = targetEvt.nonPlayerName;

			return result;
		}
	}
}