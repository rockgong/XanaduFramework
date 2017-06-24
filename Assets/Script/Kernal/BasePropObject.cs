using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    interface IPropObjectHost
    {
        //TODO:
    }

    abstract class BasePropObject : IPropObject
    {
        public string name { get; set; }
        private IPropObjectHost _host;

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
        public virtual Vector3 viewPosition
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

        public float yaw { get; set; }

        public abstract void PlayAnimation(string name);

        public void SetHost(IPropObjectHost host)
        {
            _host = host;
        }

        public abstract void Initialize(PropObjectDesc desc);
        public abstract void Uninitialize(PropObjectDesc desc);
    }
}