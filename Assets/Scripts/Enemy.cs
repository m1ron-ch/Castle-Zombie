using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyObj;
    [SerializeField] private Material _skinDeath;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _health = 100;

    private Transform _target;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Rigidbody _rg;
    private int _damage = 12;
    private bool _isAttack;
    private bool _isAttackCooldown;

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

        if (other.gameObject.TryGetComponent(out Player player))
        {
            _isAttack = true;
            Attack(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _isAttack = false;
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
        _enemyObj.GetComponent<Renderer>().material = _skinDeath;
        transform.transform.GetComponent<BoxCollider>().enabled = false;

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

    private void Attack(Player player)
    {
        if ((_target == null) || !_isAttack || _isAttackCooldown) { return; }

        SoundManager.Instance.PlayZombieAttack();
        _animator.SetTrigger(Key.Animations.Attack.ToString());
        player.Damage(_damage);

        StartCoroutine(AttackCooldown());

        if (_isAttack) { Util.Invoke(this, () => Attack(player), 2f); }
    }

    private IEnumerator AttackCooldown()
    {
        _isAttackCooldown = true;
        yield return new WaitForSeconds(1);

        _isAttackCooldown = false;
    }
}
