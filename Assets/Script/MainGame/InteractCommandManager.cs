using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public interface BaseInteractCommandData
    {

    }

    public interface IInteractCommandDatabase
    {
        BaseInteractCommandData GetDataById(int id);
    }

    class InteractCommandManager
    {
        private InteractGameState _gameState;
        private IInteractCommandDatabase _database;
        private InteractCommandBuilder _builder;

        public void Initialize(InteractGameState state, IInteractCommandDatabase db, InteractCommandBuilder builder)
        {
            _gameState = state;
            _database = db;
            _builder = builder;
        }

        public BaseInteractCommand GetCommandById(int id)
        {
            BaseInteractCommandData data = _database.GetDataById(id);
            if (data != null)
                return _builder.Build(data);
            return null;
        }
    }
}