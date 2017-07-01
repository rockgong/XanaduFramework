using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventSetStringValue : BaseCommonEvent
	{
		public int index;
		public string targetValue;
	}

	class MainGameCommandSetStringValue : BaseMainGameCommand
	{
		public int index = 0;
		public string targetValue = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.valueManager.SetStringValue(index, targetValue);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventSetStringValue targetEvt = (CommonEventSetStringValue)evt;

            MainGameCommandSetStringValue result = new MainGameCommandSetStringValue();
            result.index = targetEvt.index;
            result.targetValue = targetEvt.targetValue;

            return result;
        }
	}
}