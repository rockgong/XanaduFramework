using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventNonPlayerSetDialog : BaseCommonEvent
	{
		public int nonPlayerId = 0;
		public int dialogId = 0;
	}

	class MainGameCommandNonPlayerSetDialog : BaseMainGameCommand
	{
		public int nonPlayerId = 0;
		public int dialogId = 0;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.nonPlayerManager.SetInteractCommandIdByName(nonPlayerId, dialogId);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt)
        {
            CommonEventNonPlayerSetDialog targetEvt = (CommonEventNonPlayerSetDialog)evt;

            MainGameCommandNonPlayerSetDialog result = new MainGameCommandNonPlayerSetDialog();
            result.nonPlayerId = targetEvt.nonPlayerId;
            result.dialogId = targetEvt.dialogId;

            return result;
        }
	}
}