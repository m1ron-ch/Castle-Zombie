using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _bulletDamage = 5;

    protected Enemy _target;
    private float fireDelay = 0.4f;
    private float _bulletSpeed = 0.4f;
    private float _nextFireTime;

    private void Update()
    {
        _target = _enemyManager.GetNearestEnemy(transform.position);

        if (_target == null)
            return;

        if (Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireDelay;
        }
    }

    public virtual void Init(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void Init(EnemyManager enemyManager, Quaternion defaultPosition)
    {
        _enemyManager = enemyManager;
        // _defaultTurretPosition = defaultPosition;
    }

    protected virtual void Shoot()
    {
        Bullet bullet = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
        // bullet.transform.position = _rotationTurret.transform.position;
        bullet.SetDamage(_bulletDamage);

        Vector3 targetVelocity = _target.Rigidbody.velocity;
        Vector3 targetPosition = PredictFuturePosition(transform.position, _bulletSpeed, targetVelocity);

        bullet.transform.DOMove(targetPosition, _bulletSpeed).SetEase(Ease.Linear).OnComplete(() => DestroyBullet(bullet.gameObject));
    }

    private Vector3 PredictFuturePosition(Vector3 startPosition, float bulletSpeed, Vector3 targetVelocity)
    {
        float timeToHit = Vector3.Distance(startPosition, _target.transform.position) / bulletSpeed;
        Vector3 futurePosition = _target.transform.position + targetVelocity * timeToHit;
        return futurePosition;
    }

    private void DestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }
}
