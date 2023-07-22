using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Room> _rooms = new();

    private GameManager _gameManager;

    public List<Room> Rooms => _rooms;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        foreach (Room room in _rooms)
            room.Init(this);
    }

    public void CloseAllDors()
    {
        foreach (Room room in _rooms)
        {
            room.Door.Close();
        }
    }

    public void IsAllDoorsClose()
    {
        int count = 0;
        foreach (Room room in _rooms)
            if (room.Door.IsClose)
                count++;
        
        if (count == _rooms.Count)
        {
            _gameManager.EndTimerHiding();
        }
    }
}
