using System;
using UnityEngine;

namespace Game.Minigames
{
    [Serializable]
    public class TorsoGameRoundData : MinigameRoundData
    {
        [Header("Gameplay")]
        public float moveSpeedMultiplier = 1f;
        public float instabilityMultiplier = 1f;

        [Header("Objectives")]
        public int _imagesToCollect = 10;

        [Header("Difficulty")]
        public float difficultyMultiplier = 1f;
        public int _maxBangAmount;
        public float _dropSpeedMultiplier = 1f;
    }
}
