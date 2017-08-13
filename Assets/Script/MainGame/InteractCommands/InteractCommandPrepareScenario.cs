using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;

namespace MainGame
{
    public class InteractCommandPrepareScenarioData : BaseInteractCommandData
    {
        public int stageId = 0;
        public int scenarioId = 0;
        public string sceneName = string.Empty;
        public string stagePointName = string.Empty;
        public int type = 0; // 0 : current; 1 : new
    }

    class InteractCommandPrepareScenario : BaseInteractCommand
    {
        public int stageId = 0;
        public int scenarioId = 0;
        public string sceneName = string.Empty;
        public string stagePointName = string.Empty;
        public int type = 0; // 0 : current; 1 : new

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (_interactGameStateHost != null)
                _interactGameStateHost.OnPrepareScenario(stageId, scenarioId, sceneName, stagePointName, type);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandPrepareScenarioData target = (InteractCommandPrepareScenarioData)data;
            InteractCommandPrepareScenario result = new InteractCommandPrepareScenario();

            result.stageId = target.stageId;
            result.scenarioId = target.scenarioId;
            result.sceneName = target.sceneName;
            result.stagePointName = target.stagePointName;
            result.type = target.type;

            return result;
        }
    }
}