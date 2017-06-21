using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;

public class MonoGameKernalTest : MonoBehaviour {
	public Vector3 cameraOffset;

    private IGameKernal gameKernal;

    private MainGameState _mainGameState = new MainGameState();
    private InteractGameState _interactGameState = new InteractGameState();

	void Start () {
        gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc());

        GameObject playerPrototype = Resources.Load<GameObject>("Character/Player/Player");

        gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

        GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

        gameKernal.SetupStage(new StageDesc(stagePrototype));

        ICamera cam = gameKernal.GetCamera();

        cam.lookPosition = Vector3.zero;

        cam.offset = cameraOffset;

        gameKernal.Startup();

        gameKernal.SetGameState(_mainGameState);
	}

	void OnGUI()
	{
		if (GUILayout.Button("MainGameState"))
			gameKernal.SetGameState(_mainGameState);

		if (GUILayout.Button("InteractGameState"))
			gameKernal.SetGameState(_interactGameState);
	}
}
