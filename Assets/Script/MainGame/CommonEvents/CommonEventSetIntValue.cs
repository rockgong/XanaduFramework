using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventSetIntValue : BaseCommonEvent
	{
		public int index;
		public int targetValue;
	}

	class MainGameCommandSetIntValue : BaseMainGameCommand
	{
		public int index = 0;
		public int targetValue = 0;

        public override void Excute(MainGameCommandManager mgr)
        {
        	mgr.valueManager.SetIntValue(index, targetValue);
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
        {
            CommonEventSetIntValue targetEvt = (CommonEventSetIntValue)evt;

            MainGameCommandSetIntValue result = new MainGameCommandSetIntValue();
            result.index = targetEvt.index;
            result.targetValue = targetEvt.targetValue;

            return result;
        }
	}
}