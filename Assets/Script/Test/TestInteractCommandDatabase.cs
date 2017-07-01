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
                result.members = new BaseInteractCommandData[2];
                InteractCommandDialogData dia = new InteractCommandDialogData();
                dia.content = "Going to change dialog";
                result.members[0] = dia;
                dia = new InteractCommandDialogData();
                dia.content = "Hit me again";
                dia.commandTarget = 0;
                result.members[1] = dia;
                return result;
            }
            else if (id == 2)
            {
                InteractCommandDialogData result = new InteractCommandDialogData();
                result.content = "Changed";
                return result;
            }
            else if (id == 3)
            {
                InteractCommandSelectData result = new InteractCommandSelectData();
                result.title = "My Select";
                result.options = new string[] { "First", "Second" };
                result.optionCommands = new BaseInteractCommandData[]
                {
                    new InteractCommandGroupData()
                    {
                        members = new BaseInteractCommandData[]
                        {
                            new InteractCommandDialogData()
                            {
                                content = "You Select First"
                            },
                            new InteractCommandDialogData()
                            {
                                content = "Thank you"
                            },
                            new InteractCommandCommonEventData()
                            {
                                eventName = "SelectFirst"
                            }
                        }
                    },
                    new InteractCommandGroupData()
                    {
                        members = new BaseInteractCommandData[]
                        {
                            new InteractCommandDialogData()
                            {
                                content = "You Select Second"
                            },
                            new InteractCommandMessageData()
                            {
                                content = "Go ahead"
                            }
                        }
                    }
                };
                return result;
            }

        	return null;
        }
	}
}