using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Player _player;
    [SerializeField] private int _enemyCount = 2;

    [Header("Spawn Points For Enemy")]
    [SerializeField] private List<Transform> _spawPoints;

    private GameManager _gameManager;
    private static List<Enemy> _enemies = new List<Enemy>();
    private float _nearestDistance = 10;
    private bool _isSpawning;

    public static List<Enemy> Enemies => _enemies;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        StartSpaw();
    }

    public void StartSpaw()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy(_spawPoints[i].position);
        }
    }

    public void StopSpaw()
    {

    }

    public void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.SetTarget(_player.transform);
        _enemies.Add(enemy);
    }

    public Enemy GetNearestEnemy(Vector3 position)
    {
        Enemy nearestEnemy = null;
        float nearestDistance;

        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        nearestDistance = screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1 ? 4 : 7;

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
