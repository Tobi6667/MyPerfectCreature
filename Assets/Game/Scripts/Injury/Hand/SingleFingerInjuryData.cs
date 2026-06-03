using UnityEngine;

namespace Game.Body
{

    [System.Serializable]
    public class SingleFingerInjuryData
    {
        public EFingerTypes finger;

        [Range(0f, 1f)] public float strengthLoss;
        [Range(0f, 1f)] public float rangeLoss;
        [Range(0f, 1f)] public float noise;
        [Range(0f, 1f)] public float tremor;
        [Range(0f, 1f)] public float delay;
        [Range(0f, 1f)] public float stiffness;

    }
}