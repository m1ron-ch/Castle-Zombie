using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    [SerializeField] private UnityEngine.Sprite _backpack;

    [Header("Resources")]
    [SerializeField] private UnityEngine.Sprite _wood;
    [SerializeField] private UnityEngine.Sprite _rock;
    [SerializeField] private UnityEngine.Sprite _coins;
    [SerializeField] private UnityEngine.Sprite _gems;

    public UnityEngine.Sprite Wood => _wood;
    public UnityEngine.Sprite Rock => _rock;
    public UnityEngine.Sprite Coins => _coins;
    public UnityEngine.Sprite Gems => _gems;

    public UnityEngine.Sprite GetSprite(Key.ResourcePrefs resource)
    {
        switch (resource)
        {
            case Key.ResourcePrefs.Wood:
                return _wood;
            case Key.ResourcePrefs.Rock:
                return _rock;
            case Key.ResourcePrefs.Coins:
                return _coins;
            case Key.ResourcePrefs.Gems:
                return _gems;
            default:
                return null;
        }
    }
}
