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

    class CommonIntegerLiteralEval : BaseCommonIntegerEval
    {
        public int val;

        public override int Evaluate()
        {
            return val;
        }

        public static BaseCommonIntegerEval BuildHandler(BaseCommonInteger data, MainGameIntegerBuilder builder)
        {
            CommonIntegerLiteral target = (CommonIntegerLiteral)data;
            CommonIntegerLiteralEval result = new CommonIntegerLiteralEval();

            result.val = target.val;

            return result;
        }
    }
}