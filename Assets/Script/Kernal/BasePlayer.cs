using UnityEngine;

namespace GameKernal
{
    // For interact with the player host
    interface IPlayerHost
    {
        // TODO
    }

    abstract class BasePlayer : IPlayerCharacter
    {
        private IPlayerHost _host;

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

        public float yaw{get; set;}
        public float velocity{get; set;}

        public void SetHost(IPlayerHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(PlayerCharacterDesc desc);
        public abstract void Uninitialize();
    }
}