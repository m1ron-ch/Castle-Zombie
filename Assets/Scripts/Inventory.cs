using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private UIInventory _ui;

    private int _capacity;
    private int _defaultMax = 60;
    private int _maxCapacity = 320;

    public int FreeSpaceInBackpack => _maxCapacity - _capacity;

    private void Awake()
    {
        _capacity = PlayerPrefs.GetInt(Key.Prefs.InvetoryCapacity.ToString(), 0);
        _maxCapacity = PlayerPrefs.GetInt(Key.Prefs.MaxInvetoryCapacity.ToString(), _defaultMax);

        RefreshUI();
    }

    public bool Add(int value)
    {
        if (_capacity + value > _maxCapacity)
            return false;

        _capacity += value;

        SaveCapacity();
        RefreshUI();

        return true;
    }

    public bool Remove(int value)
    {
        if (_capacity - value < 0)
            return false;

        _capacity -= value;

        SaveCapacity();
        RefreshUI();

        return true;
    }

    public void AddMaxCapacity(int value)
    {
        if (value < 0)
            return;

        _maxCapacity += value;

        SaveMaxCapacity();
    }

    private void RefreshUI()
    {
        _ui.Refresh(_capacity, _maxCapacity);
    }

    private void SaveCapacity()
    {
        PlayerPrefs.SetInt(Key.Prefs.InvetoryCapacity.ToString(), _capacity);
    }

    private void SaveMaxCapacity()
    {
        PlayerPrefs.SetInt(Key.Prefs.MaxInvetoryCapacity.ToString(), _maxCapacity);
    }
}
