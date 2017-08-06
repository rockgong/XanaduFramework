using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class CommonEventInlineUI : BaseCommonEvent
	{
		public string uiName;
		public BaseCommonEvent onEnd;
	}

	class MainGameCommandInlineUI : BaseMainGameCommand
	{
		public string uiName;
		public BaseMainGameCommand onEnd;

		public override void Excute(MainGameCommandManager mgr)
		{
			if (mgr.inlineUIView != null)
			{
				mgr.inlineUIView.ShowUI(uiName, () => 
				{
					if (onEnd != null)
						onEnd.Excute(mgr);
				}
				);
			}
		}

		public static BaseMainGameCommand BuildHandler(BaseCommonEvent evt, MainGameCommandBuilder builder)
		{
			CommonEventInlineUI targetEvt = (CommonEventInlineUI)evt;

			MainGameCommandInlineUI result = new MainGameCommandInlineUI();
			result.uiName = targetEvt.uiName;
			if (targetEvt.onEnd != null)
				result.onEnd = builder.Build(targetEvt.onEnd);

			return result;
		}
	}
}