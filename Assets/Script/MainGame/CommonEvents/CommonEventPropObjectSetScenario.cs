using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventPropObjectSetScenario : BaseCommonEvent
	{
		public int propObjectId = 0;
		public int scenarioId = 0;
		public string scenarioSceneName = string.Empty;
		public string scenarioStagePointName = string.Empty;
	}

	class MainGameCommandPropObjectSetScenario : BaseMainGameCommand
	{
		public int propObjectId = 0;
		public int scenarioId = 0;
		public string scenarioSceneName = string.Empty;
		public string scenarioStagePointName = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.propObjectManager.SetPropObjectScenario(propObjectId, scenarioId, scenarioSceneName, scenarioStagePointName);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventPropObjectSetScenario targetEvt = (CommonEventPropObjectSetScenario)evt;

            MainGameCommandPropObjectSetScenario result = new MainGameCommandPropObjectSetScenario();
            result.propObjectId = targetEvt.propObjectId;
            result.scenarioId = targetEvt.scenarioId;
            result.scenarioSceneName = targetEvt.scenarioSceneName;
            result.scenarioStagePointName = targetEvt.scenarioStagePointName;

            return result;
        }
	}
}