using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    public class MonoDelegate : MonoBehaviour
    {
        private System.Action _onUpdate;
        private int _updateType = 0; // 0 : update ; 1 : lateUpdate

        public static MonoDelegate Create(System.Action update, string name = "_Delegate", int updateType = 0)
        {
            GameObject obj = new GameObject(name);
            MonoDelegate result = obj.AddComponent<MonoDelegate>();
            result.SetUpdate(update, updateType);
            return result;
        }

        public void SetUpdate(System.Action action, int updateType)
        {
            _onUpdate = action;
            _updateType = updateType;
        }

        private void Update()
        {
            if (_onUpdate != null && _updateType == 0)
                _onUpdate();
        }

        private void LateUpdate()
        {
            if (_onUpdate != null && _updateType == 1)
                _onUpdate();
        }
    }
}