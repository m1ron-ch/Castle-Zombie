using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private List<PointForBuilding> _pointsForBuildings = new();
    [SerializeField] private Transform _gunPosition;

    [HideInInspector] public bool IsUse;
    [HideInInspector] public Humanoid StakeOut;

    private RoomManager _roomManager;

    public Door Door => _door;
    public Transform GunPosition => _gunPosition;

    private void Start()
    {
        InitPointsForBuildings();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHumanoid humanoid))
        {
            IsUse = true;

            _door.Close();

            if (other.gameObject.TryGetComponent(out Player player))
            {
                if (StakeOut != null)
                {
                    StakeOut.GetComponent<PlayerAI>().FindAnotherRoom();
                }

                ShowAllPointsForBuildings();
            }
        }
    }

    public void Init(RoomManager roomManager)
    {
        _roomManager = roomManager;
        _door.Init(roomManager);
    }

    public void RemovePoint(PointForBuilding point)
    {
        _pointsForBuildings.Remove(point);
    }

    private void InitPointsForBuildings()
    {
        foreach (PointForBuilding point in _pointsForBuildings)
            point.SetRoom(this);
    }

    private void ShowAllPointsForBuildings()
    {
        foreach (PointForBuilding point in _pointsForBuildings)
            point.gameObject.SetActive(true);
    }
}
