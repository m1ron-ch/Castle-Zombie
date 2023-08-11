using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConverResource : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform _addPoint;
    [SerializeField] private Transform _getPoint;

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

            if (ResourceController.RemoveResource(_from, _costFrom))
                ResourceController.AddResource(_to, _costTo);

            yield return new WaitForSeconds(0.3f);
        }

    }
}
