using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _health = 100;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _damage = 5;

    public NavMeshAgent Agent => _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();  

        _healthBar.SetMaxHealth(_maxHealth);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            Damage(bullet.Damage);
            bullet.Destroy();
        }
    }

    public void MoveTo(Vector3 position)
    {
        _agent.destination = position;
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _animator.SetBool(Key.Animations.Walking.ToString(), false);
    }

    public void Health(float value)
    {
        if (_health + value >= _maxHealth) _health = _maxHealth;
        else _health += value;
    }

    public void AddMaxHealt(float value)
    {
        _maxHealth += value;
    }

    public void Damage(float value)
    {
        if (_health - value > 0)
            _health -= value;
        else 
            Death();

        _healthBar.SetHealth(value, _health);
    }

    public void Death()
    {
        EnemyManager.Enemies.Remove(this);
        Destroy(gameObject);
    }
}
