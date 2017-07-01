using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class MainGameCommandBuilder
    {
        private MainGameIntegerBuilder _integerBuilder;
        public MainGameIntegerBuilder integerBuilder
        {
            get
            {
                return _integerBuilder;
            }
        }

        private Dictionary<System.Type, System.Func<BaseCommonEvent, MainGameCommandBuilder, BaseMainGameCommand>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonEvent, MainGameCommandBuilder, BaseMainGameCommand>>();

        public void Initialize()
        {
            _handlers[typeof(CommonEventNonPlayerSetDialog)] = MainGameCommandNonPlayerSetDialog.BuildHandler;
            _handlers[typeof(CommonEventGroup)] = MainGameCommandGroup.BuildHandler;
            _handlers[typeof(CommonEventPredicate)] = MainGameCommandPredicate.BuildHandler;
            _handlers[typeof(CommonEventSetIntValue)] = MainGameCommandSetIntValue.BuildHandler;
            _handlers[typeof(CommonEventSetStringValue)] = MainGameCommandSetStringValue.BuildHandler;

            if (_integerBuilder == null)
            {
                _integerBuilder = new MainGameIntegerBuilder();
                _integerBuilder.Initialize();
            }
        }

        public BaseMainGameCommand Build(BaseCommonEvent evt)
        {
            System.Type evtType = evt.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](evt, this);

            return null;
        }
    }
}