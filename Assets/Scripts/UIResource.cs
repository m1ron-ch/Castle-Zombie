using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{
    [Header("Coins")]
    [SerializeField] private Image _coinsImage;
    [SerializeField] private TMP_Text _coins;

    [Header("Gems")]
    [SerializeField] private Image _gemsImage;
    [SerializeField] private TMP_Text _gems;

    [Header("Rock")]
    [SerializeField] private Image _rocksImage;
    [SerializeField] private TMP_Text _rock;

    [Header("Wood")]
    [SerializeField] private Image _woodImage;
    [SerializeField] private TMP_Text _wood;

    #region MonoBehaviour
    private void Awake()
    {
        _coinsImage.sprite = Sprite.Instance.Coins;
        _gemsImage.sprite = Sprite.Instance.Gems;
        _rocksImage.sprite = Sprite.Instance.Rock;
        _woodImage.sprite = Sprite.Instance.Wood;
    }
    #endregion

    public void RefreshUI(Key.ResourcePrefs resourceKey, int value)
    {
        switch (resourceKey)
        {
            case Key.ResourcePrefs.Wood:
                RefreshWood(value);
                break;
            case Key.ResourcePrefs.Rock:
                RefreshRock(value);
                break;
            case Key.ResourcePrefs.Coins:
                RefreshCoins(value);
                break;
            case Key.ResourcePrefs.Gems:
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
        Resize(_woodImage.transform);
    }

    private void RefreshRock(int value)
    {
        _rock.text = value.ToString();
        Resize(_rocksImage.transform);
    }

    private void RefreshCoins(int value)
    {
        _coins.text = value.ToString();
        Resize(_coinsImage.transform);
    }

    private void RefreshGem(int value)
    {
        _gems.text = value.ToString();
        Resize(_gemsImage.transform);
    }

    private void Resize(Transform obj)
    {
        obj.DOScale(1.15f, 0.2f).OnComplete(() => obj.DOScale(1f, 0.1f));
    }
}
