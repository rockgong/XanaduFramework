using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKernal
{
    [System.Serializable]
    public class StagePointEntry
    {
        public string name;
        public Transform trans;
    }

    public class MonoStage : MonoBehaviour
    {
        public bool enableFog = true;
        public StagePointEntry[] points;
        
        private void Start()
        {
            RenderSettings.fog = enableFog;
        }

        public Transform GetPointTrans(string name)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].name == name)
                    return points[i].trans;
            }

            return null;
        }
    }
}