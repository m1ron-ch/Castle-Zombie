using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGunShop : MonoBehaviour
{
    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
