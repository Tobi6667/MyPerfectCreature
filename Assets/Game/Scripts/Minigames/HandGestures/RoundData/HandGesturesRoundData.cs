using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandGesturesRoundData : MinigameRoundData
{
    public List<EHandGestures> handGestures;

    [Header("Gesture Match")]
    public float gestureMatchThreshold = 1.5f;
    public int requiredCorrectGestures = 3;
    public float gestureMatchTimer;
    public int maxWrongCount = 3;
    public float resetTime = 0.5f;
    public float gestureInterval = 0;
}
