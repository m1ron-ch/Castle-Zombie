using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BuildingRow> _buildings = new();

    private int _currentBuildingHierarchy;

    private bool _isLastBuildingsHierarchy => _currentBuildingHierarchy > _buildings.Count - 1;

    private void Start()
    {
        ResetJSON();

        _currentBuildingHierarchy = PlayerPrefs.GetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);

        Init();
        HideAllBuildings();
        Load();
        ShowCurrentBuildings(_currentBuildingHierarchy);
    }

    // BUILD VERSION 
    public void ResetJSON()
    {
        PlayerPrefs.SetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);
        PlayerPrefs.SetString(Key.Prefs.Buldings.ToString(), null);

        Save();
    }
    // 

    public void Save()
    {
        List<BuildingRowData> buildingRowsData = new List<BuildingRowData>();
        foreach (BuildingRow row in _buildings)
        {
            BuildingRowData rowData = new BuildingRowData();
            rowData.Buildings = new List<BuildingData>();

            foreach (BuildingPoint building in row.Buildings)
            {
                BuildingData buildingData = new BuildingData();
                buildingData.Name = building.Building.name;
                buildingData.IsBuild = building.IsBuild;
                buildingData.BuildingCost = building.Cost;
                buildingData.IsNecessarilyBuildToNextHierarchy = building.Building.IsNecessarilyBuildToNextHierarchy;

                rowData.Buildings.Add(buildingData);
            }

            buildingRowsData.Add(rowData);
        }

        string json = JsonConvert.SerializeObject(buildingRowsData, Formatting.Indented);
        PlayerPrefs.SetString(Key.Prefs.Buldings.ToString(), json);

        IsAllBuildingsBuildInHierarchy();

        Debug.Log(json);
    }

    private void Load()
    {
        string json = PlayerPrefs.GetString(Key.Prefs.Buldings.ToString());
        if (json == "")
            Save();

        List<BuildingRowData> buildingsData = JsonConvert.DeserializeObject<List<BuildingRowData>>(json);
        Debug.Log("Load()\n" + json);

        for (int i = 0; i < buildingsData.Count; i++)
        {
            for (int j = 0; j < buildingsData[i].Buildings.Count; j++)
            {
                _buildings[i].Buildings[j].Init(buildingsData[i].Buildings[j].BuildingCost);
                if (buildingsData[i].Buildings[j].IsBuild)
                {
                    _buildings[i].Buildings[j].Build(true);
                }
            }
        }

        IsAllBuildingsBuildInHierarchy();
    }

    private void Init()
    {
        foreach (BuildingRow row in _buildings)
        {
            foreach (BuildingPoint building in row.Buildings)
            {
                building.Init(this);
            }
        }
    }

    private bool IsAllBuildingsBuildInHierarchy()
    {
        if (_isLastBuildingsHierarchy)
            return false;

        bool flag = true;
        foreach (BuildingPoint buildingPoint in _buildings[_currentBuildingHierarchy].Buildings)
        {
            if (!buildingPoint.IsBuild)
            {
                buildingPoint.Show();
            }

            if (buildingPoint.Building.IsNecessarilyBuildToNextHierarchy && !buildingPoint.IsBuild)
            {
                flag = false;
            }
        }            

        if (flag)
        {
            NextBuildingHierarchy();
            ShowCurrentBuildings(_currentBuildingHierarchy);
        }


        return flag;
    }

    private void ShowCurrentBuildings(int index)
    {
        if (_currentBuildingHierarchy > _buildings.Count - 1)
            return;

        foreach (BuildingPoint building in _buildings[index].Buildings)
                building.Show();
    }

    private void HideAllBuildings()
    {
        foreach (BuildingRow row in _buildings)
            foreach (BuildingPoint building in row.Buildings)
                building.Hide();
    }

    private void NextBuildingHierarchy()
    {
        _currentBuildingHierarchy++;
        PlayerPrefs.SetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), _currentBuildingHierarchy);
    }

}

[System.Serializable]
public class BuildingRow
{
    public List<BuildingPoint> Buildings = new();
}

[System.Serializable]
public class BuildingRowData
{
    public List<BuildingData> Buildings = new List<BuildingData>();
}

[System.Serializable]
public class BuildingData
{
    public string Name;
    public bool IsBuild;
    public bool IsNecessarilyBuildToNextHierarchy;
    public List<BuildingPointCost> BuildingCost;
}