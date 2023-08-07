using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private Building _building;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private int _cost;

    private BuildingManager _buildingManager;
    private int _payment = 1;
    private bool _isAddCoin;
    private bool _isBuild;

    public Building Building => _building;
    public int Cost => _cost;
    public bool IsBuild => _isBuild;

    #region MonoBehaviour
    private void Awake()
    {
        RefreshUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _isAddCoin = true;
            StartCoroutine(AddCoin());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isAddCoin = false;
    }
    #endregion

    public void Init(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
    }

    public void Init(int cost)
    {
        _cost = cost;
        RefreshUI();
    }

    public void Build(bool isLoad = false)
    {
        _cost = 0;
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

    private void RefreshUI()
    {
        _costText.text = _cost.ToString();
        _costText.transform.DOScale(1.2f, 0.3f).OnComplete(() => _costText.transform.DOScale(1, 0.2f));
    }

    private IEnumerator AddCoin()
    {
        int allCoins = ResourceController.Coins;
        while (allCoins - _payment >= _cost)
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
        }
    }
}
