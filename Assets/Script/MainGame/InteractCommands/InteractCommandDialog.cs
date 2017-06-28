using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public class InteractCommandDialogData : BaseInteractCommandData
    {
        public string content = string.Empty;
        public Vector2 position = new Vector2(0.0f, 0.0f);
    }

    class InteractCommandDialog : BaseInteractCommand
    {
        public string content = string.Empty;
        public Vector2 position = new Vector2(0.0f, 0.0f);

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            view.ShowDialog(content, position);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return view.viewState == InteractView.ViewState.None;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandDialogData target = (InteractCommandDialogData)data;
            InteractCommandDialog result = new InteractCommandDialog();

            result.content = target.content;
            result.position = target.position;

            return result;
        }
    }
}