using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class Enemy : MonoBehaviour
{
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
    }

    private void FixedUpdate()
    {
        if ((_target != null) && (Vector3.Distance(transform.position, _target.position) < 7))
        {
            MoveToTarget();
        }
        else
        {
            Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            Damage(bullet.Damage);
            bullet.Destroy();
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void MoveToTarget()
    {
        _agent.destination = _target.position;
        _agent.isStopped = false;
        _animator.SetBool(Key.Animations.Walking.ToString(), true);
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _animator.SetBool(Key.Animations.Walking.ToString(), false);
        _animator.SetBool(Key.Animations.Idle.ToString(), false);
    }

    public void Wait()
    {

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
    }

    public void Death()
    {
        _target = null;
        transform.GetComponent<BoxCollider>().enabled = false;

        Stop();
        _animator.SetTrigger(Key.Animations.Death.ToString());

        TaskController.Instance.CompleteTask(TaskType.DestroyEnemies, 1);
        if (TaskController.Instance.Task.Type == TaskType.DestroyEnemies)
        {
            Transform nextTarget = EnemyManager.Instance.GetNearestEnemy().transform;
            PointerHelper.Instance.SetTarget(nextTarget);
        }

        EnemyManager.Enemies.Remove(this);
        Util.Invoke(this, () => Destroy(gameObject), 2);
    }
}
