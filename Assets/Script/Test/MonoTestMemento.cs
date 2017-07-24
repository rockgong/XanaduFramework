using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class MonoTestMemento : MonoBehaviour, IMainGameMemento
{
	public string[] stringValuesConfig;
	public int[] intValuesConfig;
	public int[] inventoryIdsConfig;

	public string[] stringValues
	{
		get
		{
			return stringValuesConfig;
		}
		set
		{
			stringValuesConfig = value;
		}
	}

	public int[] intValues
	{
		get
		{
			return intValuesConfig;
		}
		set
		{
			intValuesConfig = value;
		}
	}

	public int[] inventoryIds
	{
		get
		{
			return inventoryIdsConfig;
		}
		set
		{
			inventoryIdsConfig = value;
		}
	}
}
