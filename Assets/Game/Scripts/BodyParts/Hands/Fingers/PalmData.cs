using UnityEngine;

[System.Serializable]
public class PalmData
{
    public Transform palmRoot;

    [Header("Editor Controls")]
    [Range(0f, 1f)] public float curl = 0f;
    [Range(0f, 1f)] public float spread = 0f;

    [HideInInspector] public float currentCurl;
    [HideInInspector] public float currentSpread;
    [HideInInspector] public float twist;
    [HideInInspector] public Quaternion restRotation;
    [HideInInspector] public float currentTwist;
}