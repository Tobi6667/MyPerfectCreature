using Game.Body;
using Game.Main;
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
    private int _bodyCounter = 0;

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
        int requiredCount = Enum.GetValues(typeof(EBodyPartType)).Length;

        if (_bodyCounter >= 3)
        {
            GameManager.Instance.FrankReady();
        }
    }



    public void BodyPartAttached()
    {
        _bodyCounter++;
        CheckAllBodyPartsAttached();
    }

    public override Transform GetTargetOnObject(EBodyPartType type)
    {
        if (_bodyParts.TryGetValue(type, out var t))
        {
            return t;
        }

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
