using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventAddInventory : BaseCommonEvent
	{
		public int inventoryId;
		public int checkAlreadyHas = 1;
	}

	class MainGameCommandAddInventory : BaseMainGameCommand
	{
		public int inventoryId;
		public int checkAlreadyHas;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (checkAlreadyHas == 0 || mgr.inventoryManager.GetInventoryById(inventoryId) == null)
				mgr.inventoryManager.AddInventory(inventoryId);
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventAddInventory targetEvt = (CommonEventAddInventory)evt;

			MainGameCommandAddInventory result = new MainGameCommandAddInventory();
			result.inventoryId = targetEvt.inventoryId;
			result.checkAlreadyHas = targetEvt.checkAlreadyHas;

			return result;
		}
	}
}