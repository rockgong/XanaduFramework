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
        public StagePointEntry[] points;
        
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