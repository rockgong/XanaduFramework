namespace GameKernal
{
    public static class GameKernalFactory
    {
        public static IGameKernal CreateGameKernal(GameKernalDesc desc)
        {
            GameKernal gameKernal = new GameKernal();
            gameKernal.Initialize(desc);

            return gameKernal;
        }
    }
}