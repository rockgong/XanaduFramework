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
            if (name == "TestEvent")
            {
                TestCommonEvent evt = new TestCommonEvent();
                evt.integer = 20;
                evt.strName = "MyAge";

                return evt;
            }

            return null;
        }
    }
}