using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;

namespace MainGame
{
    public class InteractCommandDialogData : BaseInteractCommandData
    {
        public BaseCommonString content = null;
        public int commandTarget = 1; // ) : Player; 1 : NonPlayer; 2 : PropObject
    }

    class InteractCommandDialog : BaseInteractCommand
    {
        public BaseCommonStringEval content = null;
        public CommandTarget commandTarget = CommandTarget.NonPlayer;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            view.ShowDialog(content.GetString());
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return view.viewState == InteractView.ViewState.None;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandDialogData target = (InteractCommandDialogData)data;
            InteractCommandDialog result = new InteractCommandDialog();

            result.content = builder.mainGameStringBuilder.Build(target.content, builder.mainGameStringBuilder);
            result.commandTarget = (CommandTarget)target.commandTarget;

            return result;
        }
    }
}