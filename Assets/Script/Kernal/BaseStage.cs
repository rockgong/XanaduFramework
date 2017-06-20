namespace GameKernal
{
    interface IStageHost
    {

    }

    abstract class BaseStage : IStage
    {
        private IStageHost _host;
        
        public void SetHost(IStageHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(StageDesc desc);
        public abstract void Uninitialize();
    }
}