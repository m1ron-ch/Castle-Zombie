using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIResource : MonoBehaviour
{
    [SerializeField] private TMP_Text _wood;
    [SerializeField] private TMP_Text _rock;
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private TMP_Text _gem;

    public void RefreshUI(Key.Prefs resourceKey, int value)
    {
        switch (resourceKey)
        {
            case Key.Prefs.Wood:
                RefreshWood(value);
                break;
            case Key.Prefs.Rock:
                RefreshRock(value);
                break;
            case Key.Prefs.Coins:
                RefreshCoins(value);
                break;
            case Key.Prefs.Gem:
                RefreshGem(value);
                break;
            default:
                Debug.LogWarning("Unknown resource key: " + resourceKey);
                break;
        }
    }

    private void RefreshWood(int value)
    {
        _wood.text = value.ToString();
    }

    private void RefreshRock(int value)
    {
        _rock.text = value.ToString();
    }

    private void RefreshCoins(int value)
    {
        _coins.text = value.ToString();
    }

    private void RefreshGem(int value)
    {
        _gem.text = value.ToString();
    }
}
