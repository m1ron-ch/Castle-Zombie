using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BuildingRow> _buildings = new();

    private int _currentBuildingHierarchy;

    private static BuildingManager s_instance;
    private bool _isLastBuildingsHierarchy => _currentBuildingHierarchy > _buildings.Count - 1;

    public List<BuildingRow> BuildingRows => _buildings;
    public static BuildingManager Instance => s_instance;

    #region MonoBehaviour
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetJSON();

        _currentBuildingHierarchy = PlayerPrefs.GetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);

        Init();
        HideAllBuildings();
        Load();
        ShowCurrentBuildings(_currentBuildingHierarchy);
    }
    #endregion

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

            foreach (BuildingPoint building in row.BuildingPoints)
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
    }

    private void Load()
    {
        string json = PlayerPrefs.GetString(Key.Prefs.Buldings.ToString());
        if (json == "")
        {
            Save();
            return;
        }

        List<BuildingRowData> buildingsData = JsonConvert.DeserializeObject<List<BuildingRowData>>(json);
        Debug.Log("Buildings Load()\n" + json);

        for (int i = 0; i < buildingsData.Count; i++)
        {
            for (int j = 0; j < buildingsData[i].Buildings.Count; j++)
            {
                _buildings[i].BuildingPoints[j].Init(buildingsData[i].Buildings[j].BuildingCost);
                if (buildingsData[i].Buildings[j].IsBuild)
                {
                    _buildings[i].BuildingPoints[j].Build(true);
                }
            }
        }

        IsAllBuildingsBuildInHierarchy();
    }

    private void Init()
    {
        foreach (BuildingRow row in _buildings)
        {
            foreach (BuildingPoint building in row.BuildingPoints)
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
        foreach (BuildingPoint buildingPoint in _buildings[_currentBuildingHierarchy].BuildingPoints)
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
        {
            return;
        }

        foreach (BuildingPoint buildingPoint in _buildings[index].BuildingPoints)
        {
            buildingPoint.Show();
        }
    }

    private void HideAllBuildings()
    {
        foreach (BuildingRow row in _buildings)
            foreach (BuildingPoint building in row.BuildingPoints)
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
    public List<BuildingPoint> BuildingPoints = new();
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