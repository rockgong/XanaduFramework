using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class MainGameIntegerBuilder
    {
        private Dictionary<System.Type, System.Func<BaseCommonInteger, MainGameIntegerBuilder, BaseMainGameInteger>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonInteger, MainGameIntegerBuilder, BaseMainGameInteger>>();

        public void Initialize()
        {
            _handlers[typeof(CommonIntegerLiteral)] = MainGameIntegerLiteral.BuildHandler;
            _handlers[typeof(CommonIntegerFromValue)] = MainGameIntegerFromValue.BuildHandler;
        }

        public BaseMainGameInteger Build(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            System.Type evtType = data.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](data, this);

            return null;
        }
    }
}