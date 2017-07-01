using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    interface ISaveLoadSystem
    {
        ValueManager valueManager { get; set; }
        void Save();
        void Load();
    }

    interface IValueManagerListener
    {
        // type : 0:string, 1:int
        void OnValueChanged(int type, int index);
    }

    class ValueManager
    {
        private string[] _stringValues;
        private int[] _intValues;

        private List<IValueManagerListener> _listeners = new List<IValueManagerListener>();

        public void RegisterListener(IValueManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                    return;
            }

            _listeners.Add(listener);
        }

        public void RemoveListener(IValueManagerListener listener)
        {
            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == listener)
                {
                    _listeners.Remove(listener);
                    return;
                }
            }
        }

        public void ClearListener()
        {
            _listeners.Clear();

            return;
        }

        public void Initialize(int stringValueCapacity, int intValueCapacity)
        {
            _stringValues = new string[stringValueCapacity];
            _intValues = new int[intValueCapacity];
        }

        public string GetStringValue(int i)
        {
            if (_stringValues != null && _stringValues.Length > i)
                return _stringValues[i];

            return string.Empty;
        }

        public void SetStringValue(int index, string val)
        {
            if (_stringValues != null && _stringValues.Length > index)
            {
                if (_stringValues[index] != val)
                {
                    _stringValues[index] = val;
                    for (int i = 0; i < _listeners.Count; i++)
                        _listeners[i].OnValueChanged(0, index);
                }
            }

            return;
        }

        public int GetIntValue(int index)
        {
            if (_intValues != null && _intValues.Length > index)
                return _intValues[index];

            return 0;
        }

        public void SetIntValue(int index, int val)
        {
            if (_intValues != null && _intValues.Length > index)
            {
                if (_intValues[index] != val)
                {
                    _intValues[index] = val;
                    for (int i = 0; i < _listeners.Count; i++)
                        _listeners[i].OnValueChanged(1, index);
                }
            }

            return;
        }

        public int stringValueCapacity
        {
            get
            {
                if (_stringValues != null)
                    return _stringValues.Length;

                return 0;
            }
        }

        public int intValueCapacity
        {
            get
            {
                if (_intValues != null)
                    return _intValues.Length;

                return 0;
            }
        }
    }
}