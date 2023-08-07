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
    [SerializeField] private List<BuildingPointCost> _cost;

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
            _isAddResource = true;
            StartCoroutine(AddCoin());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isAddResource = false;
    }
    #endregion

    public void Init(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
    }

    public void Init(List<BuildingPointCost> buildingPoint)
    {
        _ui.Init(buildingPoint);
    }

    public void Build(bool isLoad = false)
    {
        // _cost = 0;
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

    private IEnumerator AddResource()
    {
        while (true)
        {
            if (!_isAddResource)
                yield break;

            yield return new WaitForSeconds(0.15f);

            foreach (BuildingPointCost cost in _cost)
            {
                ResourceController.RemoveResource(cost.Resource, _payment);
                cost.Cost -= _payment;
            }
        }
    }

    private IEnumerator AddCoin()
    {
        int allCoins = ResourceController.Coins;
        yield return null;
/*        while (allCoins - _payment >= _cost)
        {
            if (!_isAddCoin)
                yield break;

            if (_cost <= 0)
            {   
                Build();

                yield break;
            }

            yield return new WaitForSeconds(0.15f);

            allCoins = ResourceController.Coins;
            ResourceController.RemoveResource(Key.ResourcePrefs.Coins, _payment);
            _cost -= _payment;
            // _payment = _price > _payment ? _payment : _price;

            RefreshUI();
            _buildingManager.Save();
        }*/
    }
}

[System.Serializable]
public class BuildingPointCost
{
    // public Dictionary<Key.ResourcePrefs, int> Cost = new();
    public Key.ResourcePrefs Resource;
    public int Cost;
}