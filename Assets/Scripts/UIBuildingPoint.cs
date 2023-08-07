using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingPoint : MonoBehaviour
{
    [SerializeField] private UIResourceContent _resource;
    [SerializeField] private Transform _instantiatePoint;
    [SerializeField] private Sprite _sprite;


    private List<BuildingPointCost> _buildingCost = new();

    public void Init(List<BuildingPointCost> buildingCost)
    {
        _buildingCost = buildingCost;

        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (BuildingPointCost buildCost in _buildingCost)
        {
            UIResourceContent content = Instantiate(_resource, _instantiatePoint);
            content.Init(_sprite.GetSprite(buildCost.Resource), buildCost.Cost);
        }
    }
}
