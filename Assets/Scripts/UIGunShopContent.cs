using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGunShopContent : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _fireRate;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private int _maxLvl;

    private int _lvl = 1;
    private bool _isUse;

    public bool IsUse => _isUse;

    public void Select()
    {

    }

    public void Unselect()
    {

    }

    public void Upgrade(string fireRate, string damage)
    {

        _lvl++;
        RefreshUI(fireRate, damage);
    }

    public void RefreshUI(string fireRate, string damage)
    {
        _fireRate.text = fireRate;
        _damage.text = damage;
    }
}
