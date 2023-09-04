using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lair : MonoBehaviour
{
    [SerializeField] private int _numberZombiesInLair;
    [SerializeField] private int _numberZombiesNearLair;
    [SerializeField] private int _health;

    private void Start()
    {
        SpawZombieNearLair();
    }

    public void Damage(int value)
    {

    }

    public void Destroy()
    {

    }

    private void SpawZombieAfterDestoy()
    {
        for (int i = 0; i < _numberZombiesInLair; i++)
        {
            EnemyManager.Instance.SpawnEnemy(transform.position);
        }
    }

    public void SpawZombieNearLair()
    {
        for (int i = 0; i < _numberZombiesInLair; i++)
        {
            EnemyManager.Instance.SpawnEnemy(transform.position, transform);
        }
    }
}
