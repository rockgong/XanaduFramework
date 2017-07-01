using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class CommonIntegerFromValue : BaseCommonInteger
    {
        public BaseCommonInteger index;
    }

    class MainGameIntegerFromValue : BaseMainGameInteger
    {
        public BaseMainGameInteger index;

        public override int Evaluate()
        {
            index.Setup(_valueManager);
            return _valueManager.GetIntValue(index.Evaluate());
        }

        public static BaseMainGameInteger BuildHandler(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            CommonIntegerFromValue target = (CommonIntegerFromValue)data;
            MainGameIntegerFromValue result = new MainGameIntegerFromValue();

            result.index = builder.Build(target.index, builder);

            return result;
        }
    }
}