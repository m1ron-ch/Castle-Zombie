using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _sitDownPlace;
    [SerializeField] private float _bulletDamage = 5;

    private Player _player;
    private float fireDelay = 0.2f;
    private float _bulletSpeed = 1.5f;
    private float _nextFireTime;
    private bool _isCanShoot;

    private void Update()
    {
        Enemy enemy = _enemyManager.GetNearestEnemy(transform.position);

        if ((enemy != null) && _isCanShoot)
        {
            RotationToTarget(enemy.transform.position);
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + fireDelay;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (Camera.main.TryGetComponent(out FollowingCamera camera))
            {
                camera.RotationTo(transform.rotation.eulerAngles);
            }

            _player = player;
            Use();
        }
    }

    public void Shoot()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        Bullet bullet = Instantiate(_bullet, Vector3.zero, Quaternion.identity);
        bullet.transform.position = transform.position;
        bullet.SetDamage(_bulletDamage);

        Vector3 targetPosition = transform.position + transform.forward * 30f;
        bullet.transform.DOMove(targetPosition, _bulletSpeed).SetEase(Ease.Linear).OnComplete(() => DestroyBullet(bullet.gameObject));
    }

    private void DestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    private void Use()
    {
        SitDown();

        _isCanShoot = true;
    }

    private void SitDown()
    {
        float duration = 0.5f;
        
        _player.transform.DORotate(_sitDownPlace.position, duration);
        _player.transform.DOMove(_sitDownPlace.position, duration).OnComplete(() => Camera.main.GetComponent<FollowingCamera>().IsFollowing(false));
    }

    private void RotationToTarget(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.DORotateQuaternion(new Quaternion(0, rotation.y, 0, rotation.w), 0.6f).OnComplete(() => _isCanShoot = true);
    }
}
