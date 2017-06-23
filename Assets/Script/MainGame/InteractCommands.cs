using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    class InteractCommandDialog : BaseInteractCommand
    {
        public string content = string.Empty;
        public Vector2 position = new Vector2(0.0f, 0.0f);

        public override void Excute(InteractView view)
        {
            view.ShowDialog(content, position);
        }
    }

    class InteractCommandMessage : BaseInteractCommand
    {
        public string content = string.Empty;
        public override void Excute(InteractView view)
        {
            view.ShowMessage(content);
        }
    }

    class InteractCommandSelect : BaseInteractCommand
    {
        public string title;
        public string[] options;
        public System.Action<int> callback;

        public override void Excute(InteractView view)
        {
            view.ShowSelect(title, options, callback);
        }
    }
}