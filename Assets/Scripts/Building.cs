using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class Building : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<int> _prices = new();

    private int _lvl = 1;

    public GameObject Prefab => _prefab;
    public int Lvl => _lvl;
    public int Cost => _prices[_lvl - 1];

    public void Upgrade()
    {
        _lvl++;
    }
}
