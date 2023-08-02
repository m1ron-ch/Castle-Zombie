using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private TMP_Text _capacity;

    public void Refresh(int value, int max)
    {
        _capacity.text = $"{value} / {max}";
        Resize();
    }

    private void Resize()
    {
        _capacity.transform.DOScale(1.15f, 0.2f).OnComplete(() => _capacity.transform.DOScale(1f, 0.1f));
    }
}
