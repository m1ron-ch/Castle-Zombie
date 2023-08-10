using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private Building _building;
    [SerializeField] private UIBuildingPoint _ui;
    [SerializeField] private List<BuildingPointCost> _cost = new();

    private BuildingManager _buildingManager;
    private int _payment = 1;
    private bool _isAddResource;
    private bool _isBuild;

    public Building Building => _building;
    public List<BuildingPointCost> Cost => _cost;
    public bool IsBuild => _isBuild;

    #region MonoBehaviour
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            IsAtPoint(player.IsMove);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isAddResource = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            IsAtPoint(player.IsMove);
        }
    }
    #endregion

    public void Init(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
    }

    public void Init(List<BuildingPointCost> buildingPointCost)
    {
        _cost = buildingPointCost;
        _ui.Init(buildingPointCost);
    }

    public void Build(bool isLoad = false)
    {
        _isBuild = true;
        _building.Build();

        if (!isLoad)
            _buildingManager.Save();

        Destroy(gameObject);
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    private void IsAtPoint(bool isMove)
    {
        if (isMove || _isAddResource)
            return;

        _isAddResource = true;
        StartCoroutine(AddResource());
    }

    private IEnumerator AddResource()
    {
        int countResourceComplete = 0;
        while (true)
        {
            if (!_isAddResource)
            {
                _buildingManager.Save();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (BuildingPointCost cost in _cost)
            {
                if ((cost.Cost > 0) && ResourceController.RemoveResource(cost.Resource, _payment))
                {
                    cost.Cost -= _payment;
                }

                if (cost.Cost == 0)
                    countResourceComplete++;
            }

            if (countResourceComplete == _cost.Count)
            {
                _isAddResource = false;
                Build();
            }

            _ui.RefreshUI(_cost);
        }
    }
    
}

[System.Serializable]
public class BuildingPointCost
{
    public Key.ResourcePrefs Resource;
    public int Cost;
}