using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ConverResource : MonoBehaviour
{
    [SerializeField] private ResourceStorage _resourceStorage;

    [Header("Storage")]
    [SerializeField] private int _capacity = 120;
    [SerializeField] private TMP_Text _storage;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _fillImage;

    [Header("From")]
    [SerializeField] private Key.ResourcePrefs _from;
    [SerializeField] private int _costFrom;

    [Header("To")]
    [SerializeField] private Key.ResourcePrefs _to;
    [SerializeField] private int _costTo;

    private int _currentCapacity;
    private float _fillDuration = 1;
    private bool _isProcessing = false;
    private bool _isConversion = false;

    #region MonoBehaviour
    private void Awake()
    {
        _resourceStorage.Init(_to, _costTo);
        RefreshUI();
    }

    private void Start()
    {
        _icon.sprite = Sprite.Instance.GetSprite(_from);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            HandleResourceTransfer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            HandleResourceTransfer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isProcessing = false;
    }
    #endregion

    private void HandleResourceTransfer()
    {
        if (!_isProcessing)
        {
            _isProcessing = true;
            StartCoroutine(DelayedResourceTransfer());
        }
    }

    private IEnumerator DelayedResourceTransfer()
    {
        while (_isProcessing)
        {

            if (ResourceController.RemoveResource(_from, _costFrom) 
                && (_currentCapacity + _costFrom <= _capacity))
            {
                if (!_isConversion)
                {
                    _isConversion = true;
                    StartCoroutine(Conversion());
                }

                SoundManager.Instance.PlayAddResource();
                _currentCapacity += _costFrom;
                RefreshUI();
            }
            else
            {
                _isProcessing = false;
            }

            yield return new WaitForSeconds(0.12f);
        }
    }

    private IEnumerator Conversion()
    {
        float timer = 0f;
        float startFillAmount = 0f;
        float targetFillAmount = 1f;

        while (timer < _fillDuration)
        {
            timer += Time.deltaTime;
            float fillProgress = timer / _fillDuration;

            float currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, fillProgress);

            _fillImage.fillAmount = currentFillAmount;

            yield return null;
        }

        _currentCapacity -= _costFrom;
        _resourceStorage.AddResource();
        RefreshUI();

        if (_currentCapacity > 0)
        {
            StartCoroutine(Conversion());
        }
        else
        {
            targetFillAmount = 0f;
            _isConversion = false;
        }

        _fillImage.fillAmount = targetFillAmount;
    }

    private void RefreshUI()
    {
        _storage.text = $"{_currentCapacity} / {_capacity}";
        Resize(_icon.transform);
    }

    private void Resize(Transform obj)
    {
        obj.DOScale(1.15f, 0.2f).OnComplete(() => obj.DOScale(1f, 0.1f));
    }
}
