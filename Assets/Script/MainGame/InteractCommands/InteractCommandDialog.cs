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
            Vector3 worldPosition = Vector3.zero;
            if (commandTarget == CommandTarget.Player && player != null)
                worldPosition = player.viewPosition;
            else if (commandTarget == CommandTarget.NonPlayer && nonPlayer != null)
                worldPosition = nonPlayer.viewPosition;
            else if (commandTarget == CommandTarget.PropObject && prop != null)
                worldPosition = prop.viewPosition;
            Vector2 position = UIUtils.WorldPointToCanvasAnchoredPosition(worldPosition, new Vector2(1280.0f, 720.0f));
            view.ShowDialog(content.GetString(), position);
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