using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : PlayerAI
{
    [SerializeField] private List<ObjectType> _resources = new();

    private int _currentCapacity;
    private int _maxCapacity;
}
