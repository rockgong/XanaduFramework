using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    public class MonoDelegate : MonoBehaviour
    {
        private System.Action _onUpdate;

        public static MonoDelegate Create(System.Action update, string name = "_Delegate")
        {
            GameObject obj = new GameObject(name);
            MonoDelegate result = obj.AddComponent<MonoDelegate>();
            result.SetUpdate(update);
            return result;
        }

        public void SetUpdate(System.Action action)
        {
            _onUpdate = action;
        }

        private void Update()
        {
            if (_onUpdate != null)
                _onUpdate();
        }
    }
}