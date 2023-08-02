using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIResource))]  
public class ResourceController : MonoBehaviour
{
    private static Dictionary<Key.Prefs, int> resources = new Dictionary<Key.Prefs, int>();
    private static UIResource _ui;

    private void Awake()
    {
        _ui = GetComponent<UIResource>();

        resources.Add(Key.Prefs.Wood, PlayerPrefs.GetInt(Key.Prefs.Wood.ToString(), 0));
        resources.Add(Key.Prefs.Rock, PlayerPrefs.GetInt(Key.Prefs.Rock.ToString(), 0));
        resources.Add(Key.Prefs.Coins, PlayerPrefs.GetInt(Key.Prefs.Coins.ToString(), 0));
        resources.Add(Key.Prefs.Gem, PlayerPrefs.GetInt(Key.Prefs.Gem.ToString(), 0));

        RefreshUI();
    }

    #region Add Logic

    /*    public static void AddWood(int value)
        {
            AddResource(Key.Prefs.Wood, value);
        }

        public static void AddRock(int value)
        {
            AddResource(Key.Prefs.Rock, value);
        }

        public static void AddCoins(int value)
        {
            AddResource(Key.Prefs.Coins, value);
        }

        public static void AddGem(int value)
        {
            AddResource(Key.Prefs.Gem, value);
        }

        public static void RemoveWood(int value)
        {
            AddResource(Key.Prefs.Wood, -value);
        }

        public static void RemoveRock(int value)
        {
            AddResource(Key.Prefs.Rock, -value);
        }

        public static void RemoveCoins(int value)
        {
            AddResource(Key.Prefs.Coins, -value);
        }

        public static void RemoveGem(int value)
        {
            AddResource(Key.Prefs.Gem, -value);
        }*/
    #endregion

    public static void AddResource(Key.Prefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        int currentAmount = resources[resourceKey];
        resources[resourceKey] = currentAmount + value;
        _ui.RefreshUI(resourceKey, resources[resourceKey]);

        SaveResource(resourceKey, resources[resourceKey]);
    }

    public static void RemoveResource(Key.Prefs resourceKey, int value)
    {
        if (value <= 0)
            return;

        int currentAmount = resources[resourceKey];
        resources[resourceKey] = currentAmount - value;
        _ui.RefreshUI(resourceKey, resources[resourceKey]);

        SaveResource(resourceKey, resources[resourceKey]);
    }

    private void RefreshUI()
    {
        foreach (KeyValuePair<Key.Prefs, int> res in resources)
            _ui.RefreshUI(res.Key, res.Value);

/*        _ui.RefreshWood(resources[Key.Prefs.Wood]);
        _ui.RefreshRock(resources[Key.Prefs.Rock]);
        _ui.RefreshCoins(resources[Key.Prefs.Coins]);
        _ui.RefreshGem(resources[Key.Prefs.Gem]);*/
    }

    private static void SaveResource(Key.Prefs resourceKey, int value)
    {
        resources[resourceKey] = value;
        PlayerPrefs.SetInt(resourceKey.ToString(), value);
    }
}
