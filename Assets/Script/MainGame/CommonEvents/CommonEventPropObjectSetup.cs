using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventPropObjectSetup : BaseCommonEvent
	{
		public int propObjectId = 0;
		public int dialogId = 0;
        public int scenarioId = 0;
        public string scenarioSceneName = string.Empty;
        public string scenarioStagePointName = string.Empty;
        public int orderCode = 1;
	}

	class MainGameCommandPropObjectSetup : BaseMainGameCommand
	{
        public int propObjectId = 0;
        public int dialogId = 0;
        public int scenarioId = 0;
        public string scenarioSceneName = string.Empty;
        public string scenarioStagePointName = string.Empty;
        public int orderCode = 1;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.propObjectManager.SetInteractCommandIdByName(propObjectId, dialogId);
            mgr.propObjectManager.SetPropObjectScenario(propObjectId, scenarioId, scenarioSceneName, scenarioStagePointName);
            mgr.propObjectManager.SetOrderCodeIdByName(propObjectId, orderCode);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventPropObjectSetup targetEvt = (CommonEventPropObjectSetup)evt;

            MainGameCommandPropObjectSetup result = new MainGameCommandPropObjectSetup();
            result.propObjectId = targetEvt.propObjectId;
            result.dialogId = targetEvt.dialogId;
            result.scenarioId = targetEvt.scenarioId;
            result.scenarioSceneName = targetEvt.scenarioSceneName;
            result.scenarioStagePointName = targetEvt.scenarioStagePointName;
            result.orderCode = targetEvt.orderCode;

            return result;
        }
	}
}