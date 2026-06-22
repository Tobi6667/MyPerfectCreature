using Game.Body;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TableFrankenstein : LabObjectBase
{

    public static TableFrankenstein Instance;
    [SerializeField] private List<TableFrankensteinData> _map;
    private Dictionary<EBodyPartType, Transform> _bodyParts;

    public Action<EBodyPartType> _onBodypartAttached;
    private HashSet<EBodyPartType> _attachedParts = new();


    private void Awake()
    {
        Instance = this;

        _bodyParts = new Dictionary<EBodyPartType, Transform>();

        foreach (var b in _map)
        {
            _bodyParts[b.type] = b.transform;
        }
    }

    private void OnEnable()
    {
    }



 

    private void CheckAllBodyPartsAttached()
    {
        int requiredCount =
            Enum.GetValues(typeof(EBodyPartType)).Length;

        if (_attachedParts.Count >= 1)
        {
            OnAllBodyPartsAttached();
        }
    }


    private void OnAllBodyPartsAttached()
    {
        Debug.Log("ALL BODY PARTS ATTACHED — FRANKENSTEIN COMPLETE");



    }

    public override Transform GetTargetOnObject(EBodyPartType type)
    {
        if (_bodyParts.TryGetValue(type, out var t))
            return t;

        Debug.LogError($"No slot found for type: {type}");
        return null;
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckAllBodyPartsAttached();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
