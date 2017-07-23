using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventHasInventory : BaseCommonEvent
	{
		public int inventoryId;
		public BaseCommonEvent yesEvent;
		public BaseCommonEvent noEvent;
	}

	class MainGameCommandHasInventory : BaseMainGameCommand
	{
		public int inventoryId;
		public BaseMainGameCommand yesCommand;
		public BaseMainGameCommand noCommand;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (mgr.inventoryManager.GetInventoryById(inventoryId) == null)
			{
				if (noCommand != null)
					noCommand.Excute(mgr);
			}
			else
			{
				if (yesCommand != null)
					yesCommand.Excute(mgr);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventHasInventory targetEvt = (CommonEventHasInventory)evt;

			MainGameCommandHasInventory result = new MainGameCommandHasInventory();
			result.inventoryId = targetEvt.inventoryId;
			result.yesCommand = builder.Build(targetEvt.yesEvent);
			result.noCommand = builder.Build(targetEvt.noEvent);

			return result;
		}
	}
}