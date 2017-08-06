using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class ScenarioPhaseDataSerial : BaseScenarioPhaseData
	{
		public BaseScenarioPhaseData[] members;
	}

	class ScenarioPhaseSerial : BaseScenarioPhase
	{
		public BaseScenarioPhase[] members;

		private int _curIndex = 0;

		public override void Setup(IGameKernal kernal, MonoScenarioScene scene, InlineUIView iuv)
		{
			base.Setup(kernal, scene, iuv);

			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
					members[i].Setup(kernal, scene, iuv);
			}
		}

		public override void Enter()
		{
			_curIndex = 0;
			if (members != null && members.Length > 0)
				members[0].Enter();

			return;
		}

		public override bool CheckOver()
		{
			if (members != null && _curIndex < members.Length)
			{
				if (members[_curIndex].CheckOver())
				{
					members[_curIndex].Exit();
					_curIndex++;
					if (_curIndex < members.Length)
						members[_curIndex].Enter();
				}

				return false;
			}

			return true;
		}

		public override void Exit()
		{
			return;
		}

		public static BaseScenarioPhase BuildHandler(BaseScenarioPhaseData data, ScenarioPhaseBuilder builder)
		{
			ScenarioPhaseDataSerial target = (ScenarioPhaseDataSerial)data;
			ScenarioPhaseSerial result = new ScenarioPhaseSerial();

			if (target != null)
			{
				result.members = new BaseScenarioPhase[target.members.Length];
				for (int i = 0; i < target.members.Length; i++)
					result.members[i] = builder.Build(target.members[i]);
			}

			return result;
		}
	}
}