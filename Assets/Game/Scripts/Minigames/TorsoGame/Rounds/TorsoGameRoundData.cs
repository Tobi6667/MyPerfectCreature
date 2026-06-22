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
        public float dropInterval = 1f;
        public float dropSpeed = 1f;
        [Header("Objectives")]
        public int imagesToCollect = 10;

        public MoveImagesObject imageObject;

        [Header("Difficulty")]
        public float difficultyMultiplier = 1f;
        public int maxBangAmount;
        public float dropSpeedMultiplier = 1f;
    }
}
