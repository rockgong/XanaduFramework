using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonEventRemovePropObject : BaseCommonEvent
	{
		public string propObjectName;
	}

	class MainGameCommandRemovePropObject : BaseMainGameCommand
	{
		public string propObjectName;

		public override void Excute(MainGameCommandManager mgr)
		{
			IGameKernal kernal = mgr.gameKernal;
			if (kernal != null)
				kernal.RemovePropObject(propObjectName);
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventRemovePropObject targetEvt = (CommonEventRemovePropObject)evt;

			MainGameCommandRemovePropObject result = new MainGameCommandRemovePropObject();
			result.propObjectName = targetEvt.propObjectName;

			return result;
		}
	}
}