using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class MainGameCommandBuilder
    {
        private Dictionary<System.Type, System.Func<BaseCommonEvent, BaseMainGameCommand>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonEvent, BaseMainGameCommand>>();

        public void Initialize()
        {
            _handlers[typeof(TestCommonEvent)] = TestMainGameCommand.BuildHandler;
        }

        public BaseMainGameCommand Build(BaseCommonEvent evt)
        {
            System.Type evtType = evt.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](evt);

            return null;
        }
    }
}