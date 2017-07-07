using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using MainGame;
using LitJson;

namespace Helper
{
	public static class JsonHelper
	{
		public static object PolymorphReflectParse(JsonData jsonData, System.Type type)
		{
			if (type == typeof(int))
			{
				if (jsonData.IsInt)
					return (int)jsonData;
				else
					throw new System.FormatException("Json Format Error : not int");
			}
			else if (type == typeof(string))
			{
				if (jsonData.IsString)
					return (string)jsonData;
				else
					throw new System.FormatException("Json Format Error : not string");
			}
			else if (type == typeof(double))
			{
				if (jsonData.IsDouble)
					return (double)jsonData;
				else
					throw new System.FormatException("Json Format Error : not double");
			}
			else if (type == typeof(bool))
			{
				if (jsonData.IsBoolean)
					return (bool)jsonData;
				else
					throw new System.FormatException("Json Format Error : not boolean");
			}
			else if (type.GetElementType() != null)
			{
				if (jsonData.IsArray)
				{
					int elementCount = jsonData.Count;
					System.Array result = System.Array.CreateInstance(type.GetElementType(), new int[]{elementCount});
					for (int i = 0; i < elementCount; i++)
					{
						object elem = PolymorphReflectParse(jsonData[i], type.GetElementType());
						result.SetValue(elem, new int[]{i});
					}
					return result;
				}
				else
					throw new System.FormatException("Json Format Error : not array");
			}
			else
			{
				System.Type targetType = null;
				if (jsonData.Keys.Contains("$type"))
					targetType = System.Type.GetType((string)jsonData["$type"]);

				if (targetType == null)
					targetType = type;

				FieldInfo[] fields = targetType.GetFields();
				object result = System.Activator.CreateInstance(targetType);
                for (int i = 0; i < fields.Length; i++)
                {
                    try
                    {
                        fields[i].SetValue(result, PolymorphReflectParse(jsonData[fields[i].Name], fields[i].FieldType));
                    }
                    catch(System.Collections.Generic.KeyNotFoundException ex)
                    {
                        continue;
                    }
                }
				return result;
			}

			return null;
		}
	}
}