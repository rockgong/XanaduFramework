using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandWaitData : BaseInteractCommandData
	{
		public float time;
	}

    class InteractCommandWait : BaseInteractCommand
    {
        public float time;

        private float _timeCount;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            _timeCount = 0.0f;
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            _timeCount += Time.deltaTime;
            return _timeCount > time;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data)
        {
        	InteractCommandWaitData target = (InteractCommandWaitData)data;
        	InteractCommandWait result = new InteractCommandWait();

        	result.time = target.time;

        	return result;
        }
    }
}