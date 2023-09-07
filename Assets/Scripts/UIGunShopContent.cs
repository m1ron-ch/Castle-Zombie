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
}
