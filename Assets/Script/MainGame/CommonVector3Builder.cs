using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	class CommonVector3Builder
	{
		private Dictionary<System.Type, System.Func<BaseCommonVector3, CommonVector3Builder, BaseCommonVector3Eval>> _handlers = new Dictionary<System.Type, System.Func<BaseCommonVector3, CommonVector3Builder, BaseCommonVector3Eval>>();

		public void Initialize()
		{
			_handlers[typeof(CommonVector3Literal)] = CommonVector3LiteralEval.BuildHandler;
			_handlers[typeof(CommonVector3Player)] = CommonVector3PlayerEval.BuildHandler;
			_handlers[typeof(CommonVector3StagePoint)] = CommonVector3StagePointEval.BuildHandler;
			_handlers[typeof(CommonVector3Plus)] = CommonVector3PlusEval.BuildHandler;
			_handlers[typeof(CommonVector3Minus)] = CommonVector3MinusEval.BuildHandler;
			_handlers[typeof(CommonVector3Multiply)] = CommonVector3MultiplyEval.BuildHandler;
			_handlers[typeof(CommonVector3Cross)] = CommonVector3CrossEval.BuildHandler;
			_handlers[typeof(CommonVector3Clamp)] = CommonVector3ClampEval.BuildHandler;
			_handlers[typeof(CommonVector3Lerp)] = CommonVector3LerpEval.BuildHandler;
			_handlers[typeof(CommonVector3SetMag)] = CommonVector3SetMagEval.BuildHandler;
		}

		public BaseCommonVector3Eval Build(BaseCommonVector3 data)
		{
            System.Type evtType = data.GetType();
            if (_handlers.ContainsKey(evtType))
                return _handlers[evtType](data, this);

            return null;
		}
	}
}