using Game.Body;
using UnityEngine;

public abstract class LabObjectBase : MonoBehaviour
{
    public abstract void Initialize();
    public abstract Transform GetTargetOnObject(EBodyPartType type);
}
