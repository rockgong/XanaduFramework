using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventRemoveInventory : BaseCommonEvent
	{
		public int inventoryId;
	}

	class MainGameCommandRemoveInventory : BaseMainGameCommand
	{
		public int inventoryId;

		public override void Excute(MainGameCommandManager mgr)
		{
			InventoryInfo info = mgr.inventoryManager.GetInventoryById(inventoryId);
			if (info != null)
				mgr.inventoryManager.RemoveInventory(info);
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventRemoveInventory targetEvt = (CommonEventRemoveInventory)evt;

			MainGameCommandRemoveInventory result = new MainGameCommandRemoveInventory();
			result.inventoryId = targetEvt.inventoryId;

			return result;
		}
	}
}