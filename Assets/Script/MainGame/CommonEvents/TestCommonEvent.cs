using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class TestCommonEvent : BaseCommonEvent
    {
        public int integer = 10;
        public string strName = "Test Common Event";

    }

    class TestMainGameCommand : BaseMainGameCommand
    {
        public int integerValue = 10;
        public string strName = string.Empty;

        public override void Excute(MainGameCommandManager mgr)
        {
            Debug.Log(string.Format("{0} = {1}", strName, integerValue));
        }

        public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt)
        {
            TestCommonEvent targetEvt = (TestCommonEvent)evt;

            TestMainGameCommand result = new TestMainGameCommand();
            result.integerValue = targetEvt.integer;
            result.strName = targetEvt.strName;

            return result;
        }
    }
}