using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Config;

namespace MainGame
{
    public class CommonStringLiteral : BaseCommonString
    {
        public string val;
    }

    class CommonStringLiteralEval : BaseCommonStringEval
    {
        public string val;

        public override string GetString()
        {
            if (val.StartsWith("$"))
                return TextMap.Map(val.Substring(1));
            return val;
        }

        public static BaseCommonStringEval BuildHandler(BaseCommonString data, MainGameStringBuilder builder)
        {
            CommonStringLiteral target = (CommonStringLiteral)data;
            CommonStringLiteralEval result = new CommonStringLiteralEval();

            result.val = target.val;

            return result;
        }
    }
}