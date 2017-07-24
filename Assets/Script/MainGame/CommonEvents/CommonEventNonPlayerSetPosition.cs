using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventNonPlayerSetPosition : BaseCommonEvent
	{
		public int nonPlayerId = 0;
		public int stageId = 0;
        public string stagePointName = string.Empty;
        public string stageLookPointName = string.Empty;
	}

	class MainGameCommandNonPlayerSetPosition : BaseMainGameCommand
	{
		public int nonPlayerId = 0;
        public int stageId = 0;
        public string stagePointName = string.Empty;
        public string stageLookPointName = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.nonPlayerManager.SetNonPlayerPosition(nonPlayerId, stageId, stagePointName, stageLookPointName);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventNonPlayerSetPosition targetEvt = (CommonEventNonPlayerSetPosition)evt;

            MainGameCommandNonPlayerSetPosition result = new MainGameCommandNonPlayerSetPosition();
            result.nonPlayerId = targetEvt.nonPlayerId;
            result.stageId = targetEvt.stageId;
            result.stagePointName = targetEvt.stagePointName;
            result.stageLookPointName = targetEvt.stageLookPointName;

            return result;
        }
	}
}