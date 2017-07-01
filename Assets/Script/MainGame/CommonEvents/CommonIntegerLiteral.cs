using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class CommonIntegerLiteral : BaseCommonInteger
    {
        public int val;
    }

    class MainGameIntegerLiteral : BaseMainGameInteger
    {
        public int val;

        public override int Evaluate()
        {
            return val;
        }

        public static BaseMainGameInteger BuildHandler(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            CommonIntegerLiteral target = (CommonIntegerLiteral)data;
            MainGameIntegerLiteral result = new MainGameIntegerLiteral();

            result.val = target.val;

            return result;
        }
    }
}