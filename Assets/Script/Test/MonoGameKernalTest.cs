using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;

public class MonoGameKernalTest : MonoBehaviour, IInteractGameStateHost, IGameKernalHost {
	public Vector3 cameraOffset;

    private IGameKernal gameKernal;

    private MainGameState _mainGameState = new MainGameState();
    private InteractGameState _interactGameState = new InteractGameState();

	void Start () {
        gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);

        GameObject playerPrototype = Resources.Load<GameObject>("Character/Player/Player");

        gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

        GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

        gameKernal.SetupStage(new StageDesc(stagePrototype));

        INonPlayerCharacter nonPlayer = gameKernal.AddNonPlayerCharacter("nana", new NonPlayerCharacterDesc(playerPrototype));

        nonPlayer.position = new Vector3(0.0f, 0.0f, 5.0f);

        ICamera cam = gameKernal.GetCamera();

        cam.lookPosition = Vector3.zero;

        cam.offset = cameraOffset;

        gameKernal.Startup();

        gameKernal.SetGameState(_mainGameState);

        _interactGameState.SetHost(this);
	}

	void OnGUI()
	{
		if (GUILayout.Button("MainGameState"))
			gameKernal.SetGameState(_mainGameState);

		if (GUILayout.Button("InteractGameState"))
			gameKernal.SetGameState(_interactGameState);
	}

    public void OnInteractEnd()
    {
        gameKernal.SetGameState(_mainGameState);
    }

    public void OnInteract(IPlayerCharacter player, INonPlayerCharacter nonPlayer)
    {
        _interactGameState.player = player;
        _interactGameState.nonPlayer = nonPlayer;
        gameKernal.SetGameState(_interactGameState);
    }
}
