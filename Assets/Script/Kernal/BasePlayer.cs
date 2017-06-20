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

        public void SetHost(IPlayerHost host)
        {
            _host = host;

            return;
        }

        public abstract void Initialize(PlayerCharacterDesc desc);
        public abstract void Uninitialize();
    }
}