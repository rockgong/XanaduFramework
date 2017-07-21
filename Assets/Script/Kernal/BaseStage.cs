using UnityEngine;

namespace GameKernal
{
    interface IStageHost
    {

    }

    abstract class BaseStage : IStage
    {
        protected IStageHost _host;

        public abstract Vector3 GetStagePoint(string name);
        public abstract Vector3 GetStagePointSize(string name);
        public abstract void PlayerStageAnimation(string targetName, string animationStateName);

        
        public void SetHost(IStageHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(StageDesc desc, System.Action onEnd);
        public abstract void Uninitialize();
    }
}