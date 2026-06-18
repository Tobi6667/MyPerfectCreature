using UnityEngine;

[System.Serializable]
public class MinigameRoundData
{
    public string roundId;

    [TextArea]
    public string instruction;
    public bool showInstruction;
    public float duration;
    public int targetScore;

}
