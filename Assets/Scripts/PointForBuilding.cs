using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointForBuilding : MonoBehaviour
{
    [SerializeField] private UIBuildingMenu _buildingMenu;
    [SerializeField] private Transform _point;

    private Room _room;
    private GameObject _building;

    public Room Room => _room;
    public GameObject Building => _building;

    public void ShowBuildingMenu()
    {
        if (_building == null)
            _buildingMenu.ShowBuildMenu(this);
        else
            _buildingMenu.ShowBuildingMenu(this);
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }

    public void Build(GameObject building)
    {
        _building = building;
        HidePoint();
    } 

    public void DestroyObject()
    {
        Destroy(_building);
        ShowPoint();
    }

    private void ShowPoint()
    {
        _point.gameObject.SetActive(true);
    }

    private void HidePoint()
    {
        _point.gameObject.SetActive(false);
    }
}
