using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BuildingPoint : MonoBehaviour
{
    [SerializeField] private Transform _building;
    [SerializeField] private TMP_Text _priceT;
    [SerializeField] private int _price;

    private Vector3 _defaultScale;
    private int _payment = 200;
    private bool _isAddCoin;

    #region MonoBehaviour
    private void Awake()
    {
        _defaultScale = _building.localScale;
        _building.localScale = Vector3.zero;
        _building.gameObject.SetActive(false);

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

    private void RefreshUI()
    {
        _priceT.text = _price.ToString();
        _priceT.transform.DOScale(1.2f, 0.3f).OnComplete(() => _priceT.transform.DOScale(1, 0.2f));
    }

    private IEnumerator AddCoin()
    {
        int allCoins = ResourceController.Coins;
        while (allCoins - _payment >= _price)
        {
            if (!_isAddCoin)
                yield break;

            if (_price <= 0)
            {
                _building.gameObject.SetActive(true);
                _building.SetParent(null);
                _building.DOScale(_defaultScale * 1.4f, 0.35f).OnComplete(() => _building.DOScale(_defaultScale, 0.3f));
                Destroy(gameObject);

                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            allCoins = ResourceController.Coins;
            ResourceController.RemoveResource(Key.Prefs.Coins, _payment);
            _price -= _payment;
            _payment = _price > _payment ? _payment : _price;

            RefreshUI();
        }
    }
}
