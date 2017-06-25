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
        
        public void SetHost(IStageHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(StageDesc desc);
        public abstract void Uninitialize();
    }
}