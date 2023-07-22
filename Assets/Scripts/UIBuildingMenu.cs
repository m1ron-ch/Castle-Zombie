using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private Transform _buildMenu;
    [SerializeField] private Transform _buildingMenu;

    [Header("Buildings")]
    [SerializeField] private Building _turret;
    [SerializeField] private Building _mortal;

    [Header("Scripts")]
    [SerializeField] private EnemyManager _enemyManager;

    private PointForBuilding _poinForBuilding;

    public void CreateTurret()
    {
        GameObject building = Instantiate(_turret.Prefab, _poinForBuilding.transform.position, Quaternion.identity);
        building.transform.rotation = _poinForBuilding.transform.rotation;
        if (building.TryGetComponent(out Turret turret))
        {
            turret.Init(_enemyManager, _poinForBuilding.transform.rotation);
            _poinForBuilding.Build(building);
            Hide();
        }

        // _poinForBuilding.Room.RemovePoint(_poinForBuilding) ;
    }

    public void CreateMortal()
    {
        GameObject building = Instantiate(_mortal.Prefab, _poinForBuilding.transform.position, Quaternion.identity);
        if (building.TryGetComponent(out Turret turret))
        {
            // turret.Init(_enemyManager);
        }
        _poinForBuilding.Build(building);
        Hide();

        // _poinForBuilding.Room.RemovePoint(_poinForBuilding) ;
    }

    public void ShowBuildMenu(PointForBuilding point)
    {
        _buildMenu.gameObject.SetActive(true);
        _poinForBuilding = point;
    }

    public void ShowBuildingMenu(PointForBuilding point)
    {
        _buildingMenu.gameObject.SetActive(true);
        _poinForBuilding = point;
    }

    public void DestroyObject()
    {
        _poinForBuilding.DestroyObject();
        Hide();
    }

    public void Hide()
    {
        if (_buildingMenu.gameObject.activeSelf)
            _buildingMenu.gameObject.SetActive(false);
        else 
            _buildMenu.gameObject.SetActive(false);
    }
}
