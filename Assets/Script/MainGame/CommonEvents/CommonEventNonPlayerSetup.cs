using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventNonPlayerSetup : BaseCommonEvent
	{
		public int nonPlayerId = 0;
		public int dialogId = 0;
        public int scenarioId = 0;
        public string scenarioSceneName = string.Empty;
        public string scenarioStagePointName = string.Empty;
        public int orderCode = 1;
	}

	class MainGameCommandNonPlayerSetup : BaseMainGameCommand
	{
        public int nonPlayerId = 0;
        public int dialogId = 0;
        public int scenarioId = 0;
        public string scenarioSceneName = string.Empty;
        public string scenarioStagePointName = string.Empty;
        public int orderCode = 1;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.nonPlayerManager.SetInteractCommandIdByName(nonPlayerId, dialogId);
            mgr.nonPlayerManager.SetNonPlayerScenario(nonPlayerId, scenarioId, scenarioSceneName, scenarioStagePointName);
            mgr.nonPlayerManager.SetOrderCodeIdByName(nonPlayerId, orderCode);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventNonPlayerSetup targetEvt = (CommonEventNonPlayerSetup)evt;

            MainGameCommandNonPlayerSetup result = new MainGameCommandNonPlayerSetup();
            result.nonPlayerId = targetEvt.nonPlayerId;
            result.dialogId = targetEvt.dialogId;
            result.scenarioId = targetEvt.scenarioId;
            result.scenarioSceneName = targetEvt.scenarioSceneName;
            result.scenarioStagePointName = targetEvt.scenarioStagePointName;
            result.orderCode = targetEvt.orderCode;

            return result;
        }
	}
}