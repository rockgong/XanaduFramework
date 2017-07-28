using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventResult : BaseCommonEvent
	{
		public int index;
	}

	class MainGameCommandResult : BaseMainGameCommand
	{
		public int index;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (mgr.mainGameHost != null)
				mgr.mainGameHost.OnRequestResult(index);
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventResult targetEvt = (CommonEventResult)evt;

			MainGameCommandResult result = new MainGameCommandResult();
			result.index = targetEvt.index;

			return result;
		}
	}
}