using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private bool _isNecessarilyBuildToNextHierarchy = true;

    private Vector3 _defaultScale;

    public bool IsNecessarilyBuildToNextHierarchy => _isNecessarilyBuildToNextHierarchy;

    private void Awake()
    {
        _defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;

        Hide();
    }

    public void Build()
    {
        transform.SetParent(null);

        Show();
        transform.DOScale(_defaultScale * 1.4f, 0.35f).OnComplete(() => transform.DOScale(_defaultScale, 0.3f));
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
