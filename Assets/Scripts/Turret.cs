using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform _rotationTurret;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _bulletDamage = 5;

    private Quaternion _defaultTurretPosition;
    private float fireDelay = 0.2f;
    private float _bulletSpeed = 1.5f;
    private float _nextFireTime;

    private void Update()
    {
        Enemy enemy = _enemyManager.GetNearestEnemy(transform.position);

        if (enemy != null)
        {
            RotationToTarget(enemy.transform.position);
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + fireDelay;
            }
        }
        else
            RotationDefaultPosition();
    }

    public void Init(EnemyManager enemyManager, Quaternion defaultPosition)
    {
        _enemyManager = enemyManager;
        _defaultTurretPosition = defaultPosition;
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
        bullet.transform.position = _rotationTurret.transform.position;
        bullet.SetDamage(_bulletDamage);

        Vector3 targetPosition = _rotationTurret.transform.position + _rotationTurret.transform.forward * 30f;
        bullet.transform.DOMove(targetPosition, _bulletSpeed).SetEase(Ease.Linear).OnComplete(() => DestroyBullet(bullet.gameObject));
    }

    private void DestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    private void RotationToTarget(Vector3 position)
    {
        Vector3 direction = position - _rotationTurret.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        _rotationTurret.transform.DORotateQuaternion(new Quaternion(0, rotation.y, 0, rotation.w), 0.6f);
    }

    private void RotationDefaultPosition()
    {
        _rotationTurret.transform.DORotate(_defaultTurretPosition.eulerAngles, 0.6f);
    }
}
