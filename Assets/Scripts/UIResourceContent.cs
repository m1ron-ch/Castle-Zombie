using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIResourceContent : MonoBehaviour
{
    [SerializeField] private Image _image; 
    [SerializeField] private TMP_Text _cost;

    private Key.ResourcePrefs _resource;

    public Key.ResourcePrefs Resource => _resource;

    public void Init(Key.ResourcePrefs resource, int cost, UnityEngine.Sprite sprite)
    {
        _resource = resource;
        _cost.text = cost.ToString();
        _image.sprite = sprite;
    }

    public void RefreshUI(int cost)
    {
        _cost.text = cost.ToString();
    }
}
