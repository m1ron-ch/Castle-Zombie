using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRevive : MonoBehaviour
{
    [SerializeField] private Transform _button;

    private static UIRevive s_instance;

    public static UIRevive Instance => s_instance;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Show()
    {
        _button.gameObject.SetActive(true);
    }

    public void Hide() 
    {
        _button.gameObject.SetActive(false);
    }
}
