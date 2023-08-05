using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIResource))]  
public class ResourceController : MonoBehaviour
{
    [SerializeField] private static Inventory _inventory;

    private static Dictionary<Key.Prefs, int> _resources = new Dictionary<Key.Prefs, int>();
    private static UIResource _ui;

    public static int Wood => _resources[Key.Prefs.Wood];
    public static int Rock => _resources[Key.Prefs.Rock];
    public static int Coins => _resources[Key.Prefs.Coins];
    public static int Gem => _resources[Key.Prefs.Gem];

    #region MonoBehaviour
    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        _ui = GetComponent<UIResource>();


        foreach (Key.Prefs pref in Enum.GetValues(typeof(Key.Prefs)))
            _resources.Add(pref, PlayerPrefs.GetInt(pref.ToString(), 0));

        RefreshUI();
    }
    #endregion

    public static void AddResource(Key.Prefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (((resourceKey != Key.Prefs.Coins) && (resourceKey != Key.Prefs.Gem)) 
            && (!_inventory.Add(value))) 
            return;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount + value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);
    }

    public static void RemoveResource(Key.Prefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (!_inventory.Remove(value)
            && ((resourceKey != Key.Prefs.Coins) || (resourceKey != Key.Prefs.Gem)))
            return;

        int currentAmount = _resources[resourceKey];
        _resources[resourceKey] = currentAmount - value;
        _ui.RefreshUI(resourceKey, _resources[resourceKey]);

        SaveResource(resourceKey, _resources[resourceKey]);
    }

    private void RefreshUI()
    {
        foreach (KeyValuePair<Key.Prefs, int> res in _resources)
            _ui.RefreshUI(res.Key, res.Value);
    }

    private static void SaveResource(Key.Prefs resourceKey, int value)
    {
        _resources[resourceKey] = value;
        PlayerPrefs.SetInt(resourceKey.ToString(), value);
    }
}
