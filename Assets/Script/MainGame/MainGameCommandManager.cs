﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

namespace MainGame
{
    public class BaseCommonEvent
    {
        
    }

    public class BaseCommonInteger
    {

    }

    public interface ICommonEventDatabase
    {
        BaseCommonEvent GetCommonEvent(string name);
    }

    abstract class BaseMainGameCommand
    {
        public abstract void Excute(MainGameCommandManager mgr);
    }

    abstract class BaseMainGameInteger
    {
        public abstract int Evaluate();
    }

    class MainGameCommandManager
    {
        private IGameKernal _gameKernal;
        private PlayerStageManager _playerStageManager;
        private NonPlayerManager _nonPlayerManager;
        private PropObjectManager _propObjectManager;
        private TriggerManager _triggerManager;
        private MainGameCommandBuilder _builder;
        private ICommonEventDatabase _database;

        public IGameKernal gameKernal { get { return _gameKernal; } }
        public PlayerStageManager playerStageManager { get { return _playerStageManager; } }
        public NonPlayerManager nonPlayerManager { get { return _nonPlayerManager; } }
        public PropObjectManager propObjectManager { get { return _propObjectManager; } }
        public TriggerManager triggerManager { get { return _triggerManager; } }
        public MainGameCommandBuilder builder { get { return _builder; } }
        public ICommonEventDatabase database { get { return _database; } }

        public void Initialize(IGameKernal gameKernal, PlayerStageManager psMgr, NonPlayerManager npMgr, PropObjectManager poMgr, TriggerManager tMgr, MainGameCommandBuilder b, ICommonEventDatabase db)
        {
            _gameKernal = gameKernal;
            _playerStageManager = psMgr;
            _nonPlayerManager = npMgr;
            _propObjectManager = poMgr;
            _triggerManager = tMgr;
            _builder = b;
            _database = db;
        }

        public void DoCommand(string name)
        {
            BaseCommonEvent evt = database.GetCommonEvent(name);
            if (evt != null)
            {
                BaseMainGameCommand command = builder.Build(evt);
                if (command != null)
                    command.Excute(this);
            }
        }
    }
}