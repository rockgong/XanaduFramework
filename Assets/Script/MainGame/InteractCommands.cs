using System;
using System.Collections;
using System.Collections.Generic;
using GameKernal;
using UnityEngine;

namespace MainGame
{
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
    }

    class InteractCommandSelect : BaseInteractCommand
    {
        public string title;
        public string[] options;
        public System.Action<int> callback;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            view.ShowSelect(title, options, callback);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return view.viewState == InteractView.ViewState.None;
        }
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
    }
}