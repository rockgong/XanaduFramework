using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
	public class TestInteractCommandDatabase : MonoBehaviour, IInteractCommandDatabase
	{
        public BaseInteractCommandData GetDataById(int id)
        {
        	if (id == 0)
        	{
        		InteractCommandDialogData result = new InteractCommandDialogData();
        		result.content = "Hahahaha";
        		return result;
        	}
                else if (id == 1)
                {
                        InteractCommandGroupData result = new InteractCommandGroupData();
                        result.members = new BaseInteractCommandData[3];
                        InteractCommandCommonEventData mem = new InteractCommandCommonEventData();
                        mem.eventName = "Change2";
                        result.members[0] = mem;
                        InteractCommandDialogData dia = new InteractCommandDialogData();
                        dia.content = "Going to change dialog";
                        result.members[1] = dia;
                        dia = new InteractCommandDialogData();
                        dia.content = "Hit me again";
                        result.members[2] = dia;
                return result;
                }
                else if (id == 2)
                {
                        InteractCommandDialogData result = new InteractCommandDialogData();
                        result.content = "Changed";
                        return result;
                }

        	return null;
        }
	}
}