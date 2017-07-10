using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	class ScenarioPhaseBuilder
	{
        private MainGameStringBuilder _mainGameStringBuilder;
        public MainGameStringBuilder mainGameStringBuilder
        {
            get
            {
                return _mainGameStringBuilder;
            }
        }
        private Dictionary<System.Type, System.Func<BaseScenarioPhaseData, ScenarioPhaseBuilder, BaseScenarioPhase>> _handlers = new Dictionary<System.Type, System.Func<BaseScenarioPhaseData, ScenarioPhaseBuilder, BaseScenarioPhase>>();

        public void Initialize()
        {
        	_handlers[typeof(ScenarioPhaseDataWaitTime)] = ScenarioPhaseWaitTime.BuildHandler;
            _handlers[typeof(ScenarioPhaseDataSetPosition)] = ScenarioPhaseSetPosition.BuildHandler;

        	if (_mainGameStringBuilder == null)
        	{
        		_mainGameStringBuilder = new MainGameStringBuilder();
        		_mainGameStringBuilder.Initialize();
        	}
        }

        public BaseScenarioPhase Build(BaseScenarioPhaseData data)
        {
            System.Type dataType = data.GetType();
            if (_handlers.ContainsKey(dataType))
                return _handlers[dataType](data, this);

            return null;
        }
	}
}