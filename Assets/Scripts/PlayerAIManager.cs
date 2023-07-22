using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{
    [SerializeField] private PlayerAI _playerAIPrefab;
    [SerializeField] private Transform _spawPoint;

    private GameManager _gameManager;

    public GameManager GameManager => _gameManager;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void SpawnAI()
    {
        int playerRoom = Random.Range(0, _gameManager.RoomManager.Rooms.Count);
        for (int i = 0; i < _gameManager.RoomManager.Rooms.Count; i++)
        {
            if (playerRoom == i)
                continue;

            PlayerAI player = Instantiate(_playerAIPrefab, _spawPoint.position, Quaternion.identity);
            
            Room room = _gameManager.RoomManager.Rooms[i];
            room.StakeOut = player;

            player.Init(this);
            player.MoveTo(room.GunPosition.transform.position);
        }
    }
}
