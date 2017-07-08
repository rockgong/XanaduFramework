using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class CommonIntegerFromValue : BaseCommonInteger
    {
        public BaseCommonInteger index;
    }

    class CommonIntegerFromValueEval : BaseCommonIntegerEval
    {
        public BaseCommonIntegerEval index;

        public override int Evaluate()
        {
            index.Setup(_valueManager);
            return _valueManager.GetIntValue(index.Evaluate());
        }

        public static BaseCommonIntegerEval BuildHandler(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            CommonIntegerFromValue target = (CommonIntegerFromValue)data;
            CommonIntegerFromValueEval result = new CommonIntegerFromValueEval();

            result.index = builder.Build(target.index, builder);

            return result;
        }
    }
}