using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Player _player;

    [Header("Spawn Points For Enemy")]
    [SerializeField] private Transform _spawPointB;
    [SerializeField] private Transform _spawPointA;

    private static List<Enemy> _enemies = new List<Enemy>();
    private GameManager _gameManager;
    private float _nearestDistance = 10;
    private int _enemyCount = 150;

    public static List<Enemy> Enemies => _enemies;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void StartSpaw()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy(new Vector3(Random.Range(_spawPointA.position.x, _spawPointB.position.x), 0, Random.Range(_spawPointA.position.z, _spawPointB.position.z)));
        }
    }

    public void StopSpaw()
    {

    }

    public void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.MoveTo(_gameManager.RoomManager.Rooms[Random.Range(0, _gameManager.RoomManager.Rooms.Count)].Door.transform.position);
        _enemies.Add(enemy);
    }

    public Enemy GetNearestEnemy(Vector3 position)
    {
        Enemy nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Enemy enemy in _enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        return nearestDistance > _nearestDistance ? null : nearestEnemy;
    }
}
