using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class CommonVector3Player : BaseCommonVector3
	{

	}

	class CommonVector3PlayerEval : BaseCommonVector3Eval
	{
		public override Vector3 GetVector3()
		{
			IPlayerCharacter player = _gameKernal.GetPlayerCharacter();
			if (player != null)
				return player.position;
			
			return Vector3.zero;				
		}

		public static BaseCommonVector3Eval BuildHandler(BaseCommonVector3 data, CommonVector3Builder builder)
		{
			CommonVector3Player target = (CommonVector3Player)data;
			CommonVector3PlayerEval result = new CommonVector3PlayerEval();

			return result;
		}
	}
}