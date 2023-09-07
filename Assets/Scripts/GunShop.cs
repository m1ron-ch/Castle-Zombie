using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShop : MonoBehaviour
{
    [SerializeField] private UIGunShop _ui;

    #region MonoBehaviour
    private void OnTriggerEnter(Collider other)
    {
        _ui.Show();    
    }

    private void OnTriggerExit(Collider other)
    {
        _ui.Hide();
    }
    #endregion
}
