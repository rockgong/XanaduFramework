using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    abstract class BaseTrigger : ITrigger
    {
        public string name { get; set; }

        /*
        public Vector3 position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action onTriggerEnter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action onTriggerExit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        */

        public virtual Vector3 position
        {
            get
            {
                return Vector3.zero;
            }
            set
            {
                return;
            }
        }

        public virtual Vector3 size
        {
            get
            {
                return Vector3.zero;
            }
            set
            {
                return;
            }
        }

        public System.Action onTriggerEnter { get; set; }
        public System.Action onTriggerExit { get; set; }

        public abstract void Initialize(TriggerDesc desc);
        public abstract void Uninitialize();
    }
}