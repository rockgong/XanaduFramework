using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventNonPlayerSetScenario : BaseCommonEvent
	{
		public int nonPlayerId = 0;
		public int scenarioId = 0;
		public string scenarioSceneName = string.Empty;
		public string scenarioStagePointName = string.Empty;
	}

	class MainGameCommandNonPlayerSetScenario : BaseMainGameCommand
	{
		public int nonPlayerId = 0;
		public int scenarioId = 0;
		public string scenarioSceneName = string.Empty;
		public string scenarioStagePointName = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.nonPlayerManager.SetNonPlayerScenario(nonPlayerId, scenarioId, scenarioSceneName, scenarioStagePointName);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventNonPlayerSetScenario targetEvt = (CommonEventNonPlayerSetScenario)evt;

            MainGameCommandNonPlayerSetScenario result = new MainGameCommandNonPlayerSetScenario();
            result.nonPlayerId = targetEvt.nonPlayerId;
            result.scenarioId = targetEvt.scenarioId;
            result.scenarioSceneName = targetEvt.scenarioSceneName;
            result.scenarioStagePointName = targetEvt.scenarioStagePointName;

            return result;
        }
	}
}