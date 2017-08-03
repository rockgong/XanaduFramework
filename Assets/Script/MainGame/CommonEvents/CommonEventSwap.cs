using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventSwap : BaseCommonEvent
	{
		public int stageId;
		public string stagePointName;
		public string stageLookPointName;
	}

	class MainGameCommandSwap : BaseMainGameCommand
	{
		public int stageId;
		public string stagePointName;
		public string stageLookPointName;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (mgr.playerStageManager != null)
			{
				mgr.transfer.Transfer(0.3f, 0.3f, Color.black, () => 
					mgr.playerStageManager.SwapPlayer(stageId, stagePointName, stageLookPointName)
				);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventSwap targetEvt = (CommonEventSwap)evt;

			MainGameCommandSwap result = new MainGameCommandSwap();
			result.stageId = targetEvt.stageId;
			result.stagePointName = targetEvt.stagePointName;
			result.stageLookPointName = targetEvt.stageLookPointName;

			return result;
		}
	}
}