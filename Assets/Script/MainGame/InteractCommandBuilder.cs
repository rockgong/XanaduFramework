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
            _handlers[typeof(InteractCommandDialogData)] = InteractCommandDialog.BuildHandler;
            _handlers[typeof(InteractCommandMessageData)] = InteractCommandMessage.BuildHandler;
            _handlers[typeof(InteractCommandSelectData)] = InteractCommandSelect.BuildHandler;
            _handlers[typeof(InteractCommandWaitData)] = InteractCommandWait.BuildHandler;
            _handlers[typeof(InteractCommandAnimationData)] = InteractCommandAnimation.BuildHandler;
            _handlers[typeof(InteractCommandNonPlayerFaceData)] = InteractCommandDialog.BuildHandler;
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