using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIResource))]  
public class ResourceController : MonoBehaviour
{
    [SerializeField] private Transform menu;
    [SerializeField] private static Inventory _inventory;

    private static Dictionary<Key.ResourcePrefs, int> _resources = new Dictionary<Key.ResourcePrefs, int>();
    private static UIResource _ui;

    public static int Wood => _resources[Key.ResourcePrefs.Wood];
    public static int Rock => _resources[Key.ResourcePrefs.Rock];
    public static int Coins => _resources[Key.ResourcePrefs.Coins];
    public static int Gems => _resources[Key.ResourcePrefs.Gems];

    #region MonoBehaviour
    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        _ui = GetComponent<UIResource>();


        foreach (Key.ResourcePrefs pref in Enum.GetValues(typeof(Key.ResourcePrefs)))
            _resources.Add(pref, PlayerPrefs.GetInt(pref.ToString(), 0));

        RefreshUI();
    }
    #endregion

    // Build
    private bool isOpen = false;
    public void OnClickMenu()
    {
        isOpen = !isOpen;
        menu.gameObject.SetActive(isOpen);
    }

    // Add
    public void OnClickAddWood()
    {
        AddResource(Key.ResourcePrefs.Wood, 500);
    }

    public void OnClickAddRock()
    {
        AddResource(Key.ResourcePrefs.Rock, 500);
    }

    public void OnClickAddCoins()
    {
        AddResource(Key.ResourcePrefs.Coins, 500);
    }

    public void OnClickAddGems()
    {
        AddResource(Key.ResourcePrefs.Gems, 500);
    }

    // Remove
    public void OnClickRemoveWood()
    {
        RemoveResource(Key.ResourcePrefs.Wood, 500);
    }

    public void OnClickRemoveRock()
    {
        RemoveResource(Key.ResourcePrefs.Rock, 500);
    }

    public void OnClickRemoveCoins()
    {
        RemoveResource(Key.ResourcePrefs.Coins, 500);
    }

    public void OnClickRemoveGems()
    {
        RemoveResource(Key.ResourcePrefs.Gems, 500);
    }

    // Reset
    public void OnClickResetWood()
    {
        RemoveResource(Key.ResourcePrefs.Wood, Wood);
    }

    public void OnClickResetRock()
    {
        RemoveResource(Key.ResourcePrefs.Rock, Rock);
    }

    public void OnClickResetCoins()
    {
        RemoveResource(Key.ResourcePrefs.Coins, Coins);
    }

    public void OnClickResetGems()
    {
        RemoveResource(Key.ResourcePrefs.Gems, Gems);
    }
    //

    public static void AddResource(Key.ResourcePrefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (((resourceKey != Key.ResourcePrefs.Coins) && (resourceKey != Key.ResourcePrefs.Gems)) 
            && (!_inventory.Add(value))) 
            return;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount + value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);
    }

    public static bool RemoveResource(Key.ResourcePrefs resourceKey, int value)
    {
        if ((value <= 0) || (_resources[resourceKey] - value <= 0))
            return false;

        if (((resourceKey != Key.ResourcePrefs.Coins) && (resourceKey != Key.ResourcePrefs.Gems))
            && (!_inventory.Remove(value)))
            return false;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount - value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);

        return true;
    }

    private void RefreshUI()
    {
        foreach (KeyValuePair<Key.ResourcePrefs, int> res in _resources)
            _ui.RefreshUI(res.Key, res.Value);
    }

    private static void SaveResource(Key.ResourcePrefs resourceKey, int value)
    {
        _resources[resourceKey] = value;
        PlayerPrefs.SetInt(resourceKey.ToString(), value);
    }
}
