using UnityEngine;

namespace GameKernal
{
    // The Description of the Game Kernal
    public struct GameKernalDesc
    {

    }

    // The Description of the Player Character
    public struct PlayerCharacterDesc
    {
        public GameObject prototype;

        public PlayerCharacterDesc(GameObject prototype)
        {
            this.prototype = prototype;
        }
    }

    // The Description of the Non Player Character
    public struct NonPlayerCharacterDesc
    {
        public GameObject prototype;

        public NonPlayerCharacterDesc(GameObject prototype)
        {
            this.prototype = prototype;
        }
    }

    // The Description of the Prop Object
    public struct PropObjectDesc
    {

    }

    // The Description of the Stage
    public struct StageDesc
    {
        public GameObject prototype;

        public StageDesc(GameObject prototype)
        {
            this.prototype = prototype;
        }
    }

    public enum GameKernalErrorCode
    {
        OK = 0,
        NoPlayer,
    }

    public interface IGameKernalHost
    {
        void OnInteract(IPlayerCharacter player, INonPlayerCharacter nonPlayer);
    }

    // Game kernal itself
    public interface IGameKernal
    {
        GameKernalErrorCode Startup();
        GameKernalErrorCode Shutdown();
        IPlayerCharacter SetupPlayerCharacter(PlayerCharacterDesc desc);
        IPlayerCharacter GetPlayerCharacter();
        INonPlayerCharacter AddNonPlayerCharacter(string name, NonPlayerCharacterDesc desc);
        INonPlayerCharacter GetNonPlayerCharacter(string name);
        void RemoveNonPlayerCharacter(string name);
        void RemoveNonPlayerCharacter(INonPlayerCharacter handler);
        IPropObject AddPropObject(string name, PropObjectDesc desc);
        IPropObject GetPropObject(string name);
        void RemovePropObject(string name);
        void RemovePropObject(IPropObject handler);
        IStage SetupStage(StageDesc desc);
        IStage GetStage();
        ICamera GetCamera();
        void SetGameState(IGameState state);
        void SetCameraFollowPlayer(bool enable);

        void TryInteract();
    }

    // Player Character
    public interface IPlayerCharacter
    {
        Vector3 position{get; set;}
        float yaw{get; set;}
        float velocity{get; set;}
    }

    // Non Player Character
    public interface INonPlayerCharacter
    {
        Vector3 position{get; set;}
        float yaw{get; set;}
        float velocity{get; set;}
    }

    // Property Character
    public interface IPropObject
    {

    }

    // Stage
    public interface IStage
    {

    }

    //Camera
    public interface ICamera
    {
        Vector3 lookPosition{get; set;}
        Vector3 offset{get; set;}
    }

    // Game State
    public interface IGameState
    {
        void EnterState(IGameKernal kernal);
        void ExitState(IGameKernal kernal);
    }
}