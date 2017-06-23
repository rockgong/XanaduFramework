using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using MainGame;

public class MonoGameKernalTest : MonoBehaviour, IInteractGameStateHost, IGameKernalHost {
	public Vector3 cameraOffset;
    public string[] selectOptions;

    private IGameKernal gameKernal;

    private MainGameState _mainGameState = new MainGameState();
    private InteractGameState _interactGameState = new InteractGameState();
    private InteractView _interactView = new InteractView();

	void Start () {
        gameKernal = GameKernalFactory.CreateGameKernal(new GameKernalDesc(), this);

        GameObject playerPrototype = Resources.Load<GameObject>("Character/Player/Player");

        IPlayerCharacter player = gameKernal.SetupPlayerCharacter(new PlayerCharacterDesc(playerPrototype));

        GameObject stagePrototype = Resources.Load<GameObject>("Stage/TestStage");

        gameKernal.SetupStage(new StageDesc(stagePrototype));

        INonPlayerCharacter nonPlayer = gameKernal.AddNonPlayerCharacter("nana", new NonPlayerCharacterDesc(playerPrototype));

        nonPlayer.position = new Vector3(0.0f, 0.0f, 5.0f);

        nonPlayer = gameKernal.AddNonPlayerCharacter("nono", new NonPlayerCharacterDesc(playerPrototype));

        nonPlayer.position = new Vector3(0.0f, 0.0f, -5.0f);

        ICamera cam = gameKernal.GetCamera();

        cam.lookPosition = player.viewPosition;

        cam.offset = cameraOffset;

        gameKernal.Startup();

        gameKernal.SetGameState(_mainGameState);

        _interactGameState.SetHost(this);

        _interactView.Initialize();

        _interactGameState.SetInteractView(_interactView);
        _interactView.SetListener(_interactGameState);
	}

    public void OnInteractEnd()
    {
        gameKernal.SetGameState(_mainGameState);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Select"))
        {
            _interactView.ShowSelect("My Select", selectOptions, (i) =>
            {
                if (i == 0)
                    Application.Quit();
            });
        }
    }

    public void OnInteract(IPlayerCharacter player, INonPlayerCharacter nonPlayer)
    {
        _interactGameState.player = player;
        _interactGameState.nonPlayer = nonPlayer;
        gameKernal.SetGameState(_interactGameState);
    }
}
