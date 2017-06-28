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

        	return null;
        }
	}
}