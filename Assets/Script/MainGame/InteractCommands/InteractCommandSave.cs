using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;

namespace MainGame
{
    public class InteractCommandSaveData : BaseInteractCommandData
    {
        public int stageId = 1;
        public string stagePointName = string.Empty;
    }

    class InteractCommandSave : BaseInteractCommand
    {
        public int stageId = 1;
        public string stagePointName = string.Empty;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (_mainGameHost != null)
                _mainGameHost.OnRequestSaveSession(stageId, stagePointName);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (_mainGameHost != null)
                return !_mainGameHost.suspending;
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandSaveData target = (InteractCommandSaveData)data;
            InteractCommandSave result = new InteractCommandSave();

            result.stageId = target.stageId;
            result.stagePointName = target.stagePointName;

            return result;
        }
    }
}