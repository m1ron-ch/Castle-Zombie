using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assistant : PlayerAI
{
    [SerializeField] ResourceStorage _resourceStorage;
    [SerializeField] private List<ObjectType> _resources = new();

    private int _currentCapacity;
    private int _maxCapacity;

    public override void MoveTo(Vector3 position)
    {
        base.MoveTo(position);
    }

    private void AddResoruce()
    {

    }

    private void RemoveResource()
    {

    }

    private void MoveToBase()
    {

    }
}
