using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandMessageData : BaseInteractCommandData
	{
		public string content;
	}

    class InteractCommandMessage : BaseInteractCommand
    {
        public string content = string.Empty;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            view.ShowMessage(content);
        }
        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return view.viewState == InteractView.ViewState.None;
        }

        public static InteractCommandMessage BuildHandler(BaseInteractCommandData data)
        {
        	InteractCommandMessageData target = (InteractCommandMessageData)data;
        	InteractCommandMessage result = new InteractCommandMessage();

        	result.content = target.content;

        	return result;
        }
    }
}