using System;
using System.Collections;
using System.Collections.Generic;
using GameKernal;
using UnityEngine;
using Helper;

namespace MainGame
{
    enum CommandTarget
    {
        Player,
        NonPlayer,
        PropObject
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
    }
}