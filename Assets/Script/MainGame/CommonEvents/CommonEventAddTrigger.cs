using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventAddTrigger : BaseCommonEvent
	{
        public string name;
        public int stageId;
        public string stagePointName;
        public int triggerType = 0; // 0 : trigger on enter (default) ; 1 : trigger on exit
        public int scenarioId = -1;
        public string scenarioSceneName = null;
        public string scenarioStagePointName = null;
        public bool scenarioNeedTransfer = true;
        public int interactId = -1;
        public string commonEventName = null;
	}

	class MainGameCommandAddTrigger : BaseMainGameCommand
	{
        public string name;
        public int stageId;
        public string stagePointName;
        public int triggerType = 0; // 0 : trigger on enter (default) ; 1 : trigger on exit
        public int scenarioId = -1;
        public string scenarioSceneName = null;
        public string scenarioStagePointName = null;
        public bool scenarioNeedTransfer = true;
        public int interactId = -1;
        public string commonEventName = null;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.triggerManager.AddTriggerInfo(name, stageId, stagePointName, triggerType, scenarioId, scenarioSceneName, scenarioStagePointName, scenarioNeedTransfer, interactId, commonEventName);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventAddTrigger targetEvt = (CommonEventAddTrigger)evt;

            MainGameCommandAddTrigger result = new MainGameCommandAddTrigger();
            result.name = targetEvt.name;
            result.stageId = targetEvt.stageId;
            result.stagePointName = targetEvt.stagePointName;
            result.triggerType = targetEvt.triggerType;
            result.scenarioId = targetEvt.scenarioId;
            result.scenarioSceneName = targetEvt.scenarioSceneName;
            result.scenarioStagePointName = targetEvt.scenarioStagePointName;
            result.scenarioNeedTransfer = targetEvt.scenarioNeedTransfer;
            result.interactId = targetEvt.interactId;
            result.commonEventName = targetEvt.commonEventName;

            return result;
        }
	}
}