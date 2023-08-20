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

    private Dictionary<Key.ResourcePrefs, int> _payments = new();
    private BuildingManager _buildingManager;
    private bool _isAddResource;
    private bool _isBuild;
    private bool _isCage;

    public Building Building => _building;
    public List<BuildingPointCost> Cost => _cost;
    public bool IsBuild => _isBuild;

    #region MonoBehaviour
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (_building.TryGetComponent(out CagePrologBuilding cage))
            {
                if (!_isCage)
                {
                    _isCage = true;
                    _isBuild = true;

                    cage.Build();
                    _buildingManager.Save();
                    Destroy(gameObject);
                }
            }
            else
            {
                IsAtPoint(player.IsMove);
            }
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
            if (_building.TryGetComponent(out CagePrologBuilding cage))
            {
                Debug.Log("CagePrologBuilding");
                if (!_isCage)
                {
                    _isCage = true;
                    _isBuild = true;

                    cage.Build();
                    _buildingManager.Save();
                    Hide();
                    //Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("Building");
                IsAtPoint(player.IsMove);
            }
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

        foreach (BuildingPointCost cost in _cost)
        {
            int payment = (int)Mathf.Round((float)cost.Cost / 10);
            _payments.Add(cost.Resource, payment < 1 ? 1 : payment);
        }

        _ui.Init(buildingPointCost);
    }

    public void Build(bool isLoad = false)
    {
        TaskController.Instance.CompleteTask(TaskType.BuildStructure, _building);

        _isBuild = true;
        _building.Build();

        if (!isLoad)
            _buildingManager.Save();

        Hide();
        //Destroy(gameObject);
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
        while (true)
        {
            int countResourceComplete = 0;
            if (!_isAddResource)
            {
                _buildingManager.Save();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (BuildingPointCost cost in _cost)
            {
                int payment = cost.Cost > _payments[cost.Resource] ? _payments[cost.Resource] : cost.Cost;
                if ((cost.Cost > 0) && ResourceController.RemoveResource(cost.Resource, payment))
                {
                    cost.Cost -= payment;
                    SoundManager.Instance.PlayAddResource();
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