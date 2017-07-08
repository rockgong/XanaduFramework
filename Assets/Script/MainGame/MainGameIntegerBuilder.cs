using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class MainGameIntegerBuilder
    {
        private Dictionary<System.Type, System.Func<BaseCommonInteger, MainGameIntegerBuilder, BaseCommonIntegerEval>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonInteger, MainGameIntegerBuilder, BaseCommonIntegerEval>>();

        public void Initialize()
        {
            _handlers[typeof(CommonIntegerLiteral)] = CommonIntegerLiteralEval.BuildHandler;
            _handlers[typeof(CommonIntegerFromValue)] = CommonIntegerFromValueEval.BuildHandler;
        }

        public BaseCommonIntegerEval Build(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            System.Type evtType = data.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](data, this);

            return null;
        }
    }
}