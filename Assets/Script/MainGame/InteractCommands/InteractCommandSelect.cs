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
			public int[] commandIDs;
		}

	    class InteractCommandSelect : BaseInteractCommand
	    {
	        public string title;
	        public string[] options;
			public int[] commandIDs;

	        private BaseInteractCommand _selectCommand;

	        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
	        {
	            view.ShowSelect(title, options, (i) =>
	            {
	            	view.CloseSelect();

	            	if (i < commandIDs.Length)
	            	{
	            		BaseInteractCommand command = _interactCommandManager.GetCommandById(commandIDs[i]);
	            		if (command != null)
	            		{
	            			command.Initialize(_mainGameCommandManager, _interactCommandManager);
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

	        public void SelectCallback(int i)
	        {

	        }

	        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data)
	        {
	        	InteractCommandSelectData target = (InteractCommandSelectData)data;
	        	InteractCommandSelect result = new InteractCommandSelect();

	        	result.title = target.title;
	        	result.options = target.options;
	        	result.commandIDs = target.commandIDs;

	        	return result;
	        }
	    }
}