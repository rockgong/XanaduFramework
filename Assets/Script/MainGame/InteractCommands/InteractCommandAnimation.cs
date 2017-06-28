using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandAnimationData : BaseInteractCommandData
	{
        public int target; // 0 : Player; 1 : NonPlayer; 2 : PropObject
        public string animationName;
	}

	class InteractCommandAnimation : BaseInteractCommand
    {
        public CommandTarget target;
        public string animationName;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (target == CommandTarget.Player && player != null)
                player.PlayAnimation(animationName);
            else if (target == CommandTarget.NonPlayer && player != null)
                nonPlayer.PlayAnimation(animationName);
            else if (target == CommandTarget.PropObject && player != null)
                prop.PlayAnimation(animationName);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
        	InteractCommandAnimationData target = (InteractCommandAnimationData)data;
        	InteractCommandAnimation result = new InteractCommandAnimation();

        	result.target = (CommandTarget)target.target;
        	result.animationName = target.animationName;

        	return result;
        }
    }
}