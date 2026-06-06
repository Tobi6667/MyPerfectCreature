using System;
using UnityEngine;

namespace Game.Minigames
{
    [Serializable]
    public class WalkAvoidRoundData : MinigameRoundData
    {
        [Header("Gameplay")]
        public float roundDuration = 10f;
        public float moveSpeedMultiplier = 1f;
        public float instabilityMultiplier = 1f;

        [Header("Objectives")]
        public int cangetHits = 3;

        [Header("Difficulty")]
        public float difficultyMultiplier = 1f;


        public bool floorTargets = false;
    }
}
