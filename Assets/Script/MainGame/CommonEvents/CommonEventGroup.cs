using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventGroup : BaseCommonEvent
	{
		public BaseCommonEvent[] members;
	}

	class MainGameCommandGroup : BaseMainGameCommand
	{
		public BaseMainGameCommand[] members;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
					members[i].Excute(mgr);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent data, MainGameCommandBuilder builder)
		{
			CommonEventGroup target = (CommonEventGroup)data;

            MainGameCommandGroup result = new MainGameCommandGroup();
			if (target.members != null)
			{
				result.members = new BaseMainGameCommand[target.members.Length];
				for (int i = 0; i < target.members.Length; i++)
				{
					result.members[i] = builder.Build(target.members[i]);
				}
			}

			return result;
		}
	}
}