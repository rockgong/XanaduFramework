using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public interface ITransfer
	{
		void Transfer(float introTime, float stayTime, Color color, System.Action action);
	}
}