using System;
using UnityEngine;


namespace Game.Minigames
{
    [Serializable]
    public class LegGameRoundData : MinigameRoundData
    {
        [Header("Difficulty")]
        public float rootMoveMultiplier = 1f;
        public float randomPushForce = 2f;
        public float randomPushIntervalMin = 0.4f;
        public float randomPushIntervalMax = 1.2f;
        public float failDistance = 1.5f;
        public float gravity = 8f;
        public float stiffness = 22f;
        public float damping = 7f;
        public float maxOffset = 1.5f;
        public float controlResponsiveness = 1f;
    }
}