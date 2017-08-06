using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventStageAnimation : BaseCommonEvent
	{
		public int stageId;
		public string stagePointName;
		public string animationName;
	}

	class MainGameCommandStageAnimation : BaseMainGameCommand
	{
		public int stageId;
		public string stagePointName;
		public string animationName;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (mgr.playerStageManager != null)
			{
				mgr.playerStageManager.SetStagePointAnimation(stageId, stagePointName, animationName);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventStageAnimation targetEvt = (CommonEventStageAnimation)evt;

			MainGameCommandStageAnimation result = new MainGameCommandStageAnimation();
			result.stageId = targetEvt.stageId;
			result.stagePointName = targetEvt.stagePointName;
			result.animationName = targetEvt.animationName;

			return result;
		}
	}
}