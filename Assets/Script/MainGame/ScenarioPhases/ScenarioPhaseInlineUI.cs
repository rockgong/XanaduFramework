using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	public class ScenarioPhaseDataInlineUI : BaseScenarioPhaseData
	{
		public string uiName = string.Empty;
		public int waitForEnd = 0;
	}

	class ScenarioPhaseInlineUI : BaseScenarioPhase
	{
		public string uiName = string.Empty;
		public int waitForEnd = 0;

		private bool showing = false;

		public override void Enter()
		{
			showing = _inlineUIView.ShowUI(uiName, () =>
			{
				showing = false;
			});
		}

		public override bool CheckOver()
		{
			if (waitForEnd != 0)
				return !showing;

			return true;
		}

		public override void Exit()
		{
			return;
		}

		public static BaseScenarioPhase BuildHandler(BaseScenarioPhaseData data, ScenarioPhaseBuilder builder)
		{
			ScenarioPhaseDataInlineUI target = (ScenarioPhaseDataInlineUI)data;
			ScenarioPhaseInlineUI result = new ScenarioPhaseInlineUI();

			result.uiName = target.uiName;
			result.waitForEnd = target.waitForEnd;

			return result;
		}
	}
}