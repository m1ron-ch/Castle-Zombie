using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Bullet _bullet;

    [Header("Params")]
    [SerializeField] private float _bulletDamage = 5;
    [SerializeField] protected float fireDelay = 0.4f;
    [SerializeField] protected float _bulletSpeed = 0.4f;

    protected Enemy _target;
    private bool _isCanFire = true;
    private float _nextFireTime;

    public virtual void Init(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void Active()
    {
        _isCanFire = true;
    }

    public void Deactivate()
    {
        _isCanFire = false;
    }

    protected void FindTarget()
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

    protected virtual void Shoot()
    {
        if (!_isCanFire) return;

        SoundManager.Instance.PlayPersonGun();

        Bullet bullet = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
        bullet.transform.position = _shootingPoint.transform.position;
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
