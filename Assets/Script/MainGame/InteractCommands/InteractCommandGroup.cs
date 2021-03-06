﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public class InteractCommandGroupData : BaseInteractCommandData
    {
        public BaseInteractCommandData[] members;
    }

    class InteractCommandGroup : BaseInteractCommand
    {
        public BaseInteractCommand[] members;

        private int _curIndex = 0;

        public override void Setup(MainGameCommandManager mgcMgr, IMainGameHost mgh, IInteractGameStateHost igsh)
        {
            base.Setup(mgcMgr, mgh, igsh);
            if (members == null)
                return;
            for (int i = 0; i < members.Length; i++)
                members[i].Setup(_mainGameCommandManager, _mainGameHost, _interactGameStateHost);
        }

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (members != null && members.Length > 0)
            {
            	_curIndex = 0;
            	members[0].Excute(view, player, nonPlayer, prop);
            }
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (members == null || _curIndex >= members.Length)
            	return true;

           	if (members[_curIndex].CheckOver(view, player, nonPlayer, prop))
           	{
           		_curIndex++;
           		if (_curIndex < members.Length)
           			members[_curIndex].Excute(view, player, nonPlayer, prop);
           	}

           	return false;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandGroupData target = (InteractCommandGroupData)data;
            InteractCommandGroup result = new InteractCommandGroup();

            if (target.members != null)
            {
            	result.members = new BaseInteractCommand[target.members.Length];
            	for (int i = 0; i < target.members.Length; i++)
            	{
            		BaseInteractCommand newMember = builder.Build(target.members[i]);
            		if (newMember != null)
            			result.members[i] = newMember;
            	}
            }

            return result;
        }
    }
}