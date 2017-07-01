using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class CommonIntegerFromValue : BaseCommonInteger
    {
        public int index;
    }

    class MainGameIntegerFromValue : BaseMainGameInteger
    {
        public int index;

        public override int Evaluate()
        {
            return _valueManager.GetIntValue(index);
        }

        public static BaseMainGameInteger BuildHandler(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            CommonIntegerFromValue target = (CommonIntegerFromValue)data;
            MainGameIntegerFromValue result = new MainGameIntegerFromValue();

            result.index = target.index;

            return result;
        }
    }
}