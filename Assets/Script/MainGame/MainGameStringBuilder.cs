using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class MainGameStringBuilder
    {
        private Dictionary<System.Type, System.Func<BaseCommonString, MainGameStringBuilder, BaseCommonStringEval>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonString, MainGameStringBuilder, BaseCommonStringEval>>();

        public void Initialize()
        {
            _handlers[typeof(CommonStringLiteral)] = CommonStringLiteralEval.BuildHandler;
        }

        public BaseCommonStringEval Build(BaseCommonString data, MainGameStringBuilder builder)
        {
            System.Type evtType = data.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](data, this);

            return null;
        }
    }
}