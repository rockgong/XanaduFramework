using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventPropObjectSetDialog : BaseCommonEvent
	{
		public int propObjectId = 0;
		public int dialogId = 0;
	}

	class MainGameCommandPropObjectSetDialog : BaseMainGameCommand
	{
		public int propObjectId = 0;
		public int dialogId = 0;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.propObjectManager.SetInteractCommandIdByName(propObjectId, dialogId);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventPropObjectSetDialog targetEvt = (CommonEventPropObjectSetDialog)evt;

            MainGameCommandPropObjectSetDialog result = new MainGameCommandPropObjectSetDialog();
            result.propObjectId = targetEvt.propObjectId;
            result.dialogId = targetEvt.dialogId;

            return result;
        }
	}
}