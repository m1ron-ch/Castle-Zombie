using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingPoint : MonoBehaviour
{
    [SerializeField] private UIResourceContent _resource;
    [SerializeField] private Transform _instantiatePoint;

    private List<BuildingPointCost> _buildingCost = new();
    private List<UIResourceContent> _buildingResources = new();

    public void Init(List<BuildingPointCost> buildingCost)
    {
        _buildingCost = buildingCost;

        InstatiateContent();
    }

    public void RefreshUI(List<BuildingPointCost> buildingPointCost)
    {
/*        Debug.Log(buildingPointCost.Resource + ": " + buildingPointCost.Cost);
        _resource.RefreshUI(buildingPointCost.Cost);
*/
        foreach (BuildingPointCost cost in buildingPointCost)
        {
            foreach (UIResourceContent content in _buildingResources)
            {
                if (content.Resource == cost.Resource)
                {
                    content.RefreshUI(cost.Cost);
                }
            }
        }
    }

    private void InstatiateContent()
    {
        foreach (BuildingPointCost buildCost in _buildingCost)
        {
            UIResourceContent content = Instantiate(_resource, _instantiatePoint);
            content.Init(buildCost.Resource, buildCost.Cost, Sprite.Instance.GetSprite(buildCost.Resource));

            _buildingResources.Add(content);
        }
    }
}
