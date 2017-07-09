using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using GameKernal;

namespace Miscs
{
	public class MainTransfer : ITransfer
	{
		private MonoScreenCover _cover = null;

		public void Initialize()
		{
			GameObject proto = Resources.Load<GameObject>("UI/ScreenCover");
			if (proto != null)
				_cover = GameObject.Instantiate<GameObject>(proto).GetComponent<MonoScreenCover>();
		}

		public void Transfer(float introTime, float stayTime, Color color, System.Action action)
		{
			_cover.targetImage.color = new Color(color.r, color.g, color.b, 0.0f);
			_cover.transSpeed = 1.0f / introTime;
			_cover.targetAlpha = 1.0f;
			_cover.action = () =>
			{
				GameObject timerGO = new GameObject("TransferTimer");
				MonoTimer monoTimer = timerGO.AddComponent<MonoTimer>();
				monoTimer.Setup(stayTime, () =>
				{
					_cover.targetAlpha = 0.0f;
				});
				if (action != null)
					action();
			};
		}
	}
}