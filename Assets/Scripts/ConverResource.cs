using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ConverResource : MonoBehaviour
{
    [Header("Storage")]
    [SerializeField] private int _capacity = 120;
    [SerializeField] private TMP_Text _storage;
    [SerializeField] private Image _icon;

    private int _currentCapacity;

    [Header("From")]
    [SerializeField] private Key.ResourcePrefs _from;
    [SerializeField] private TMP_Text _costFromText;
    [SerializeField] private int _costFrom;

    [Header("To")]
    [SerializeField] private Key.ResourcePrefs _to;
    [SerializeField] private TMP_Text _costToText;
    [SerializeField] private int _costTo;

    private bool isProcessing = false;

    #region MonoBehaviour
    private void Awake()
    {
        _costFromText.text = _costFrom.ToString();
        _costToText.text = _costTo.ToString();

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
        isProcessing = false;
    }
    #endregion

    private void HandleResourceTransfer()
    {
        if (!isProcessing)
        {
            isProcessing = true;

            StartCoroutine(DelayedResourceTransfer());
        }
    }

    private IEnumerator DelayedResourceTransfer()
    {
        while (isProcessing)
        {

            if (ResourceController.RemoveResource(_from, _costFrom) 
                && (_currentCapacity + _costFrom <= _capacity))
            {
                SoundManager.Instance.PlayAddResource();
                _currentCapacity += _costFrom;
                RefreshUI();
            }

            yield return new WaitForSeconds(0.12f);
        }

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
