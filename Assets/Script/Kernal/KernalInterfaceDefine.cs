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
        public GameObject prototype;

        public PropObjectDesc(GameObject prototype)
        {
            this.prototype = prototype;
        }
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

    public struct TriggerDesc
    {
        public Vector3 position;
        public Vector3 size;
        
        public TriggerDesc(Vector3 position, Vector3 size)
        {
            this.position = position;
            this.size = size;
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
        void OnInteract(IPlayerCharacter player, IPropObject nonPlayer);
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
        void ClearNonPlayer();
        void RemoveNonPlayerCharacter(INonPlayerCharacter handler);
        IPropObject AddPropObject(string name, PropObjectDesc desc);
        IPropObject GetPropObject(string name);
        void RemovePropObject(string name);
        void RemovePropObject(IPropObject handler);
        void ClearPropObject();
        ITrigger AddTrigger(string name, TriggerDesc desc);
        void RemoveTrigger(string name);
        void RemoveTrigger(ITrigger handler);
        void ClearTrigger();
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
        Vector3 viewPosition{get; set;}
        float yaw{get; set;}
        float velocity{get; set;}
        void PlayAnimation(string name);
    }

    // Non Player Character
    public interface INonPlayerCharacter
    {
        string name {get;set;}
        Vector3 position{get; set; }
        Vector3 viewPosition { get; set; }
        float yaw{get; set;}
        float velocity{get; set; }
        void PlayAnimation(string name);
    }

    // Property Character
    public interface IPropObject
    {
        string name { get; set; }
        Vector3 position { get; set; }
        Vector3 viewPosition { get; set; }
        float yaw { get; set; }
        void PlayAnimation(string name);
    }

    // Stage
    public interface IStage
    {
        Vector3 GetStagePoint(string name);
        Vector3 GetStagePointSize(string name);
    }

    //Camera
    public interface ICamera
    {
        Vector3 lookPosition{get; set;}
        Vector3 offset{get; set;}
        Vector3 easingTarget{get;}
        void EasingMoveTo(Vector3 position, System.Action onFinish = null);
        Transform attachTransform{get; set;}
    }

    //Trigger
    public interface ITrigger
    {
        string name { get; set; }
        Vector3 position { get; set; }
        Vector3 size { get; set; }
        System.Action onTriggerEnter { get; set; }
        System.Action onTriggerExit { get; set; }
    }

    // Game State
    public interface IGameState
    {
        void EnterState(IGameKernal kernal);
        void ExitState(IGameKernal kernal);
    }
}