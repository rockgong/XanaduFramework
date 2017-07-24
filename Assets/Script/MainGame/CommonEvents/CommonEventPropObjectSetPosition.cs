using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventPropObjectSetPosition : BaseCommonEvent
	{
		public int propObjectId = 0;
		public int stageId = 0;
        public string stagePointName = string.Empty;
        public string stageLookPointName = string.Empty;
	}

	class MainGameCommandPropObjectSetPosition : BaseMainGameCommand
	{
		public int propObjectId = 0;
        public int stageId = 0;
        public string stagePointName = string.Empty;
        public string stageLookPointName = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.propObjectManager.SetPropObjectPosition(propObjectId, stageId, stagePointName, stageLookPointName);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventPropObjectSetPosition targetEvt = (CommonEventPropObjectSetPosition)evt;

            MainGameCommandPropObjectSetPosition result = new MainGameCommandPropObjectSetPosition();
            result.propObjectId = targetEvt.propObjectId;
            result.stageId = targetEvt.stageId;
            result.stagePointName = targetEvt.stagePointName;
            result.stageLookPointName = targetEvt.stageLookPointName;

            return result;
        }
	}
}