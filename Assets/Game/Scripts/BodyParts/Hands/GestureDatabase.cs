using Game.Body;
using System.Collections.Generic;
using UnityEngine;

public class GestureDatabase : MonoBehaviour
{
    public static GestureDatabase Instance;
    [SerializeField] private List<HandPositions> _gestures;
    private Dictionary<EHandGestures, HandPositions> _gesturesDict;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gesturesDict = new Dictionary<EHandGestures, HandPositions>();

        foreach (HandPositions handPosition in _gestures)
        {
            _gesturesDict.Add(handPosition.HandGesture, handPosition);
        }


    }

    public HandPositions GetGesture(EHandGestures gesture)
    {
        Debug.Log(_gesturesDict[gesture]);
        return _gesturesDict[gesture];
    }



}
