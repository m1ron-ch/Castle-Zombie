using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIResource))]  
public class ResourceController : MonoBehaviour
{
    [SerializeField] private static Inventory _inventory;

    private static Dictionary<Key.ResourcePrefs, int> _resources = new Dictionary<Key.ResourcePrefs, int>();
    private static UIResource _ui;

    public static int Wood => _resources[Key.ResourcePrefs.Wood];
    public static int Rock => _resources[Key.ResourcePrefs.Rock];
    public static int Coins => _resources[Key.ResourcePrefs.Coins];
    public static int Gem => _resources[Key.ResourcePrefs.Gem];

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

    public static void AddResource(Key.ResourcePrefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (((resourceKey != Key.ResourcePrefs.Coins) && (resourceKey != Key.ResourcePrefs.Gem)) 
            && (!_inventory.Add(value))) 
            return;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount + value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);
    }

    public static void RemoveResource(Key.ResourcePrefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (((resourceKey != Key.ResourcePrefs.Coins) && (resourceKey != Key.ResourcePrefs.Gem))
            && (!_inventory.Remove(value)))
            return;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount - value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);
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
