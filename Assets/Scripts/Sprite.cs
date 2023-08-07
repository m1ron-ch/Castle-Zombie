using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private UnityEngine.Sprite _wood;
    [SerializeField] private UnityEngine.Sprite _rock;
    [SerializeField] private UnityEngine.Sprite _coin;
    [SerializeField] private UnityEngine.Sprite _gem;

    public UnityEngine.Sprite GetSprite(Key.ResourcePrefs resource)
    {
        switch (resource)
        {
            case Key.ResourcePrefs.Wood:
                return _wood;
            case Key.ResourcePrefs.Rock:
                return _rock;
            case Key.ResourcePrefs.Coins:
                return _coin;
            case Key.ResourcePrefs.Gem:
                return _gem;
            default:
                return null;
        }
    }
}
