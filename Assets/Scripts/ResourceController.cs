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

    private void Awake()
    {
        _ui = GetComponent<UIResource>();
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        foreach (Key.Prefs pref in Enum.GetValues(typeof(Key.Prefs)))
            _resources.Add(pref, PlayerPrefs.GetInt(pref.ToString(), 0));

        RefreshUI();
    }

    public static void AddResource(Key.Prefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        if (!_inventory.Add(value))
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

        if (!_inventory.Remove(value))
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
