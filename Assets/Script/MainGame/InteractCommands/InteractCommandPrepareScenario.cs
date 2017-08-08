using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKernal;
using UIUtil;

namespace MainGame
{
    public class InteractCommandPrepareScenarioData : BaseInteractCommandData
    {
        public int scenarioId = 0;
        public string sceneName = string.Empty;
        public string stagePointName = string.Empty;
    }

    class InteractCommandPrepareScenario : BaseInteractCommand
    {
        public int scenarioId = 0;
        public string sceneName = string.Empty;
        public string stagePointName = string.Empty;

        public override void Excute(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            if (_interactGameStateHost != null)
                _interactGameStateHost.OnPrepareScenario(scenarioId, sceneName, stagePointName);
        }

        public override bool CheckOver(InteractView view, IPlayerCharacter player, INonPlayerCharacter nonPlayer, IPropObject prop)
        {
            return true;
        }

        public static BaseInteractCommand BuildHandler(BaseInteractCommandData data, InteractCommandBuilder builder)
        {
            InteractCommandPrepareScenarioData target = (InteractCommandPrepareScenarioData)data;
            InteractCommandPrepareScenario result = new InteractCommandPrepareScenario();

            result.scenarioId = target.scenarioId;
            result.sceneName = target.sceneName;
            result.stagePointName = target.stagePointName;

            return result;
        }
    }
}