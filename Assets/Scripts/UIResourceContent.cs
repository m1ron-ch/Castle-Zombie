using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIResourceContent : MonoBehaviour
{
    [SerializeField] private Image _image; 
    [SerializeField] private TMP_Text _cost;

    public void Init(UnityEngine.Sprite sprite, int cost)
    {
        _image.sprite = sprite;
        _cost.text = cost.ToString();
    }
}
