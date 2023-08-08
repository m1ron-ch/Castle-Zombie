using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BuildingRow> _buildings = new();

    private int _currentBuildingHierarchy;

    private void Awake()
    {
/*        ResetJSON();
        Save();*/

        _currentBuildingHierarchy = PlayerPrefs.GetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);

        Init();
        HideAllBuildings();
        Load();
        ShowCurrentBuildings(_currentBuildingHierarchy);
    }

    // TEST 
    public void ResetJSON()
    {
        PlayerPrefs.SetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);
        PlayerPrefs.SetString(Key.Prefs.Buldings.ToString(), null);
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
            return;

        List<BuildingRowData> buildingsData = JsonConvert.DeserializeObject<List<BuildingRowData>>(json);
        Debug.Log(json);

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
        bool flag = true;
        foreach (BuildingPoint buildingPoint in _buildings[_currentBuildingHierarchy].Buildings)
        {
            if (!buildingPoint.IsBuild)
            {
                buildingPoint.Show();
                flag = false;
            }
        }            

        if (flag)
            NextBuildingHierarchy();

        ShowCurrentBuildings(_currentBuildingHierarchy);

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
    public List<BuildingPointCost> BuildingCost;
}

/*public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<BuildingRow> _buildings = new();

    private int _currentBuildingHierarchy;

    private void Awake()
    {
        _currentBuildingHierarchy = PlayerPrefs.GetInt(Key.Prefs.CurrentBuildingHierarchy.ToString(), 0);

        HideAllBuildings();
        ShowCurrentBuildings(_currentBuildingHierarchy);
    }

    private void ShowCurrentBuildings(int index)
    {
        foreach (Building building in _buildings[index].buildings)
            building.Show();
    }

    private void HideAllBuildings()
    {
        foreach (BuildingRow row in _buildings)
            foreach (Building building in row.buildings)
                building.Hide();
    }
}

[System.Serializable]
public class BuildingRow
{
    public List<Building> buildings = new();
}*/
