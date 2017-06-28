using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using Helper;

namespace MainGame
{
	class InteractCommandNonPlayerFaceData : BaseInteractCommandData
	{

	}

    class InteractCommandNonPlayerFace : BaseInteractCommand
    {
        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (player != null && nonPlayer != null)
            {
                Vector3 deltaPos = player.position - nonPlayer.position;
                nonPlayer.yaw = MathHelper.Vector3ToYaw(deltaPos);
            }

        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data)
        {
        	InteractCommandNonPlayerFaceData target = (InteractCommandNonPlayerFaceData)data;
        	InteractCommandNonPlayerFace result = new InteractCommandNonPlayerFace();

        	return result;
        }
    }
}