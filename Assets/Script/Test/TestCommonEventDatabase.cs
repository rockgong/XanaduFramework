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

            return null;
        }
    }
}