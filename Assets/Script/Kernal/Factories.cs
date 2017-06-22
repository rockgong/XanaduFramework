namespace GameKernal
{
    public static class GameKernalFactory
    {
        public static IGameKernal CreateGameKernal(GameKernalDesc desc, IGameKernalHost host)
        {
            GameKernal gameKernal = new GameKernal();
            gameKernal.Initialize(desc);
            gameKernal.host = host;

            return gameKernal;
        }
    }
}