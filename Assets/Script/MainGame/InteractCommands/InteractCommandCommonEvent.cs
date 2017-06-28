using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandCommonEventData : BaseInteractCommandData
	{
		public string eventName = string.Empty;
	}

	class InteractCommandCommonEvent : BaseInteractCommand
	{
        public string eventName = string.Empty;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            _mainGameCommandManager.DoCommand(eventName);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandCommonEventData target = (InteractCommandCommonEventData)data;
            InteractCommandCommonEvent result = new InteractCommandCommonEvent();

            result.eventName = target.eventName;

            return result;
        }
	}

}