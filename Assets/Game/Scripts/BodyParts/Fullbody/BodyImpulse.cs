using UnityEngine;

public struct BodyImpulse
{
    public string target;
    public float strength;
    public float duration;
    public AnimationCurve shape;

    public float decayDelay;   // time before it starts fading
    public float priority;     // affects blending dominance
    public bool persistent;    // ignores automatic removal timing
}