using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventPredicate : BaseCommonEvent
	{
		public BaseCommonInteger predicateValue;
		public BaseCommonEvent nonZeroEvent;
		public BaseCommonEvent zeroEvent;
	}

	class MainGameCommandPredicate : BaseMainGameCommand
	{
		public BaseCommonIntegerEval predicateValue;
		public BaseMainGameCommand nonZeroCommand;
		public BaseMainGameCommand zeroCommand;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (predicateValue != null)
			{
                predicateValue.Setup(mgr.valueManager);
				int val = predicateValue.Evaluate();
				if (val != 0 && nonZeroCommand != null)
					nonZeroCommand.Excute(mgr);
				else if (zeroCommand != null)
					zeroCommand.Excute(mgr);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent data, MainGameCommandBuilder builder)
		{
			CommonEventPredicate target = (CommonEventPredicate)data;

            MainGameCommandPredicate result = new MainGameCommandPredicate();
			result.predicateValue = builder.integerBuilder.Build(target.predicateValue, builder.integerBuilder);
			result.nonZeroCommand = builder.Build(target.nonZeroEvent);
			result.zeroCommand = builder.Build(target.zeroEvent);

			return result;
		}
	}
}