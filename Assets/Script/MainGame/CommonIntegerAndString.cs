using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class BaseCommonInteger
    {

    }

    public class BaseCommonString
    {

    }

    abstract class BaseCommonIntegerEval
    {
        protected ValueManager _valueManager;

        public void Setup(ValueManager vMgr)
        {
            _valueManager = vMgr;
        }

        public abstract int Evaluate();
    }

    abstract class BaseCommonStringEval
    {
        protected ValueManager _valueManager;

        public void Setup(ValueManager vMgr)
        {
            _valueManager = vMgr;
        }

        public abstract string GetString();
    }
}