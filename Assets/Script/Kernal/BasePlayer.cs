using UnityEngine;

namespace GameKernal
{
    // For interact with the player host
    interface IPlayerHost
    {
        // TODO
    }

    abstract class BasePlayer : IPlayerCharacter, INonPlayerCharacter
    {
        private IPlayerHost _host;
        public string name{get; set;}

        // Interface implement
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
        public virtual Vector3 controlPosition
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

        public float yaw{get; set;}
        public float velocity{get; set;}
        public virtual bool visible
        {
            get
            {
                return true;
            }
            set
            {
                return;
            }
        }

        public abstract void PlayAnimation(string name);

        public void SetHost(IPlayerHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(PlayerCharacterDesc desc);
        public abstract void Initialize(NonPlayerCharacterDesc desc);
        public abstract void Uninitialize();
    }
}