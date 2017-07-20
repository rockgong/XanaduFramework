using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class MonoSelectPanel : MonoBehaviour
    {
        public Transform optionsRoot;
        public GameObject optionProto;

        public void Setup(string[] optionNames, System.Action<int> onSelect)
        {
            for (int i = 0; i < optionsRoot.childCount; i++)
                GameObject.Destroy(optionsRoot.GetChild(i).gameObject);
            
            for (int i = 0; i < optionNames.Length; i++)
            {
                GameObject optionInst = GameObject.Instantiate<GameObject>(optionProto);
                optionInst.transform.SetParent(optionsRoot, false);
                Text buttonText = optionInst.GetComponentInChildren<Text>();
                buttonText.text = optionNames[i];
                Button button = optionInst.GetComponent<Button>();
                button.onClick.AddListener(GetIndexButtonCallback(i, onSelect));
            }
        }

        private UnityEngine.Events.UnityAction GetIndexButtonCallback(int index, System.Action<int> originCallback)
        {
            return () => originCallback(index);
        }
    }
}