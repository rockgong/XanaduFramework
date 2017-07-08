using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public class BaseCommonInteger
    {

    }

    public class BaseCommonString
    {

    }

    public class BaseCommonVector3
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

    abstract class BaseCommonVector3Eval
    {
        protected IGameKernal _gameKernal;

        public void Setup(IGameKernal kernal)
        {
            _gameKernal = kernal;
        }

        public abstract Vector3 GetVector3();
    }
}