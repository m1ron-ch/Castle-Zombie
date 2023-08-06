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

    private Transform _target;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Rigidbody _rg;
    private float _damage = 5;

    public NavMeshAgent Agent => _agent;
    public Rigidbody Rigidbody => _rg;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();  
        _rg = GetComponent<Rigidbody>();

        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void FixedUpdate()
    {
        if (_target != null)
            _agent.destination = _target.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            Damage(bullet.Damage);
            bullet.Destroy();
        }
    }

    public void MoveTo(Transform target)
    {
        _target = target;
        _animator.SetBool(Key.Animations.Walking.ToString(), true);
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
