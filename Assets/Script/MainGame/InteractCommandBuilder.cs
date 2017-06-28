using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class InteractCommandBuilder
    {
        private Dictionary<System.Type, System.Func<BaseInteractCommandData, BaseInteractCommand>> _handlers = new Dictionary<System.Type, System.Func<BaseInteractCommandData, BaseInteractCommand>>();

        public void Initialize()
        {

        }

        public BaseInteractCommand Build(BaseInteractCommandData evt)
        {
            System.Type evtType = evt.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](evt);

            return null;
        }
    }
}