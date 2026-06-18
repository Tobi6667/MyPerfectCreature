using JetBrains.Annotations;
using System;
using UnityEngine;


[Serializable]
public class MinigameAudioClipsData
{


    [Header("State Clips")]
    public AudioClip _playingClip;
    public AudioClip _roundEndClip;
    public AudioClip _roundFailClip;
    public AudioClip _finishedClip;
}