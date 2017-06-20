using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;

public class MonoGameKernalTest : MonoBehaviour {
    private IGameKernal gameKernal;
	void Start () {
        gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc());

        GameObject playerPrototype = Resources.Load<GameObject>("Character/Player/Player");

        gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

        GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

        gameKernal.SetupStage(new StageDesc(stagePrototype));

        gameKernal.Startup();
	}
}
