using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class ScenarioPhaseDataParallel : BaseScenarioPhaseData
	{
		public BaseScenarioPhaseData[] members;
	}

	class ScenarioPhaseParallel : BaseScenarioPhase
	{
		public BaseScenarioPhase[] members;

		public override void Setup(IGameKernal kernal, MonoScenarioScene scene)
		{
			base.Setup(kernal, scene);

			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
					members[i].Setup(kernal, scene);
			}
		}

		public override void Enter()
		{
			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
					members[i].Enter();
			}
		}

		public override bool CheckOver()
		{
			bool result = true;

			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
				{
					result &= members[i].CheckOver();
				}
			}

			return result;
		}

		public override void Exit()
		{
			if (members != null)
			{
				for (int i = 0; i < members.Length; i++)
				{
					members[i].Exit();
				}
			}

			return ;
		}

		public static BaseScenarioPhase BuildHandler(BaseScenarioPhaseData data, ScenarioPhaseBuilder builder)
		{
			ScenarioPhaseDataParallel target = (ScenarioPhaseDataParallel)data;
			ScenarioPhaseParallel result = new ScenarioPhaseParallel();

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