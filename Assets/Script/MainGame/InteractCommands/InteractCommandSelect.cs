using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
	public class InteractCommandSelectData : BaseInteractCommandData
	{
		public string title;
		public string[] options;
		public BaseInteractCommandData[] optionCommands;
	}

	class InteractCommandSelect : BaseInteractCommand
	{
	    public string title;
	    public string[] options;
		public BaseInteractCommand[] optionCommands;

	    private BaseInteractCommand _selectCommand;

        public override void Setup(MainGameCommandManager mgcMgr, IMainGameHost mgh, IInteractGameStateHost igsh)
        {
            base.Setup(mgcMgr, mgh, igsh);

            if (optionCommands != null)
            {
                for (int i = 0; i < optionCommands.Length; i++)
                {
                    optionCommands[i].Setup(mgcMgr, mgh, igsh);
                }
            }
        }

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
	    {
	        view.ShowSelect(title, options, (i) =>
	        {
	            view.CloseSelect();

	            if (i < optionCommands.Length)
	            {
                    BaseInteractCommand command = optionCommands[i];
	            	if (command != null)
	            	{
	            		command.Excute(view, player, nonPlayer, prop);
	            	}
	            	_selectCommand = command;
	            }
	            else
	            	_selectCommand = null;
	        });

	        return;
	    }

	    public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
	    {
	        return view.viewState == InteractView.ViewState.None &&
	            (_selectCommand == null || _selectCommand.CheckOver(view, player, nonPlayer, prop));
	    }

	    public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
	    {
	        InteractCommandSelectData target = (InteractCommandSelectData)data;
	        InteractCommandSelect result = new InteractCommandSelect();

	        result.title = target.title;
	        result.options = target.options;
	        	
            if (target.optionCommands != null)
            {
            result.optionCommands = new BaseInteractCommand[target.optionCommands.Length];
            for (int i = 0; i < result.optionCommands.Length; i++)
            {
                result.optionCommands[i] = builder.Build(target.optionCommands[i]);
            }
            }

	        return result;
	    }
	}
}