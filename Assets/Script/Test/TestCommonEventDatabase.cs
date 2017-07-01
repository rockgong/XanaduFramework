using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class TestCommonEventDatabase : MonoBehaviour, ICommonEventDatabase
    {
        public BaseCommonEvent GetCommonEvent(string name)
        {
            if (name == "Change1")
            {
                CommonEventNonPlayerSetDialog evt = new CommonEventNonPlayerSetDialog();
                evt.nonPlayerId = 1;
                evt.dialogId = 1;

                return evt;
            }
            else if (name == "Change2")
            {
                CommonEventNonPlayerSetDialog evt = new CommonEventNonPlayerSetDialog();
                evt.nonPlayerId = 1;
                evt.dialogId = 2;

                return evt;
            }
            else if (name == "Change3")
            {
                CommonEventNonPlayerSetDialog evt = new CommonEventNonPlayerSetDialog();
                evt.nonPlayerId = 2;
                evt.dialogId = 3;

                return evt;
            }
            else if (name == "Update")
            {
                CommonEventPredicate evt = new CommonEventPredicate();
                CommonIntegerFromValue ifv = new CommonIntegerFromValue();
                ifv.index = 0;
                evt.predicateValue = ifv;
                CommonEventNonPlayerSetDialog evtnpsd = new CommonEventNonPlayerSetDialog();
                evtnpsd.nonPlayerId = 1;
                evtnpsd.dialogId = 0;
                evt.nonZeroEvent = evtnpsd;
                evtnpsd = new CommonEventNonPlayerSetDialog();
                evtnpsd.nonPlayerId = 1;
                evtnpsd.dialogId = 2;
                evt.zeroEvent = evtnpsd;

                return evt;
            }
            else if (name == "SelectFirst")
            {
                CommonEventSetIntValue evt = new CommonEventSetIntValue();
                evt.index = 0;
                evt.targetValue = 1;

                return evt;
            }

            return null;
        }
    }
}