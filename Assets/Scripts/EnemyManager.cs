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

    private static EnemyManager s_instance;
    private static List<Enemy> _enemies = new List<Enemy>();
    private float _nearestDistance = 10;
    private bool _isSpawning;

    public static List<Enemy> Enemies => _enemies;
    public static EnemyManager Instance => s_instance;

    #region
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
        SpawnEnemy(_spawPoints[0].position);
        StartCoroutine(Spaw());
    }
    #endregion

    public IEnumerator Spaw()
    {
        while (true)
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                SpawnEnemy(_spawPoints[i].position);
            }

            yield return new WaitForSeconds(7);
        }
    }

    public void StopSpaw()
    {

    }

    public void ResetTargetAllEnemy()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.SetTarget(null);
        }
    }

    public void FindPlayerForAllEnemy()
    {
        foreach(Enemy enemy in _enemies)
        {
            enemy.SetTarget(_player.transform);
        }
    }

    public void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.SetTarget(_player.transform);
        _enemies.Add(enemy);
    }

    public Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Enemy enemy in _enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
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

        if (TaskController.Instance.Task.Type == TaskType.DestroyEnemies)
        {
            if (nearestDistance < _nearestDistance && nearestEnemy != null)
            {
                PointerHelper.Instance.SetTarget(nearestEnemy.transform);
            }
        }

        return nearestDistance > _nearestDistance ? null : nearestEnemy;
    }
}
