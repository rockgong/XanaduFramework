using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandGetInvData : BaseInteractCommandData
	{
		public string content;
        public float time = 2.0f;
	}

    class InteractCommandGetInv : BaseInteractCommand
    {
        public string content = string.Empty;
        public float time = 2.0f;

        private float _timeCnt;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            view.ShowGetInv(content);
            _timeCnt = time;
        }
        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            _timeCnt -= Time.deltaTime;
            if (_timeCnt > 0.0f)
                return false;

            view.CloseGetInv();

            return true;
        }

        public static InteractCommandGetInv BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
        	InteractCommandGetInvData target = (InteractCommandGetInvData)data;
        	InteractCommandGetInv result = new InteractCommandGetInv();

            result.content = target.content;
        	result.time = target.time;

        	return result;
        }
    }
}