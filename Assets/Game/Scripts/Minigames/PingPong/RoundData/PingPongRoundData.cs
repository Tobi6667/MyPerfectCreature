using System;
using UnityEngine;

[Serializable]
public class PingPongRoundData : MinigameRoundData
{
    public int requiredHits;
    public float ballSpeed;
    public int ballsToSpawn;

    [Header("PINGPONG GAME")]
    public float reactionSpeed = 12f;

    [Header("Strike")]
    public float strikeOffsetZ = 0.4f;
    public float strikeDistance = 1.2f;
    public float strikeSpeed = 10f;

    [Header("Recoil")]
    public float recoilDistance = 0.6f;
    public float recoilRecoverSpeed = 5f;
    public float stunTime = 0.15f;
    public float _pushVelocity;
    public float _pushOffset;
}