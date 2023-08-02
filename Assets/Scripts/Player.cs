using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Player : Humanoid
{
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _personalWeapon;

    private enum Status
    {
        None, Mine, Attack,
    }

    private Rigidbody _rb;
    private Animator _animator;
    private Transform _resource;
    private Key.Animations _currentAnimation;
    private Status _status = Status.None;
    private Coroutine _attack;
    private float _speed = 5;
    private int _damage = 10;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _animator.SetBool(_currentAnimation.ToString(), _joystick.IsTouch);

        Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;
        _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);

        #region переделать
        Enemy enemy = _enemyManager.GetNearestEnemy(transform.position);
        if (enemy != null)
        {
            _status = Status.Attack;
            _personalWeapon.gameObject.SetActive(true);

            _currentAnimation = Key.Animations.PistolRunning;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), true);
            _animator.SetBool(Key.Animations.Idle.ToString(), false);
        }
        else
        {
            _status = Status.None;
            _personalWeapon.gameObject.SetActive(false);

            _currentAnimation = Key.Animations.Running;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), false);
            _animator.SetBool(Key.Animations.PistolRunning.ToString(), false);
            _animator.SetBool(Key.Animations.Idle.ToString(), true);
        }
        #endregion

        if (direction != Vector3.zero)
        {
            Vector3 relativePos = transform.position;
            relativePos.Set(_joystick.Horizontal, 0, _joystick.Vertical);
            transform.rotation = enemy != null ? Quaternion.LookRotation(enemy.transform.position - transform.position) : Quaternion.LookRotation(relativePos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ObjectType objectType))
        {
            switch (objectType.Type)
            {
                case Key.ObjectType.Tree:
                case Key.ObjectType.Rock:
                    if ((_resource == other.gameObject.transform) || (_status == Status.Attack))
                        return;

                    if (other.gameObject.TryGetComponent(out Resource resource))
                    {
                        _status = Status.Mine;
                        _resource = other.gameObject.transform;
                        _currentAnimation = Key.Animations.Chop;

                        _attack = StartCoroutine(Attack(resource));
                    }
                    break;
                case Key.ObjectType.MachineGun:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _status = Status.None;
        _resource = null;

        StopCoroutine();
        _animator.ResetTrigger(_currentAnimation.ToString());
    }

    private IEnumerator Attack(Resource resource)
    {
        while (true)
        {
            if (!resource.Damage(_damage))
            {
                StopCoroutine();
            }

            _animator.SetTrigger(_currentAnimation.ToString());

            yield return new WaitForSeconds(1f);
        }
    }

    private void StopCoroutine()
    {
        if (_attack != null)
            StopCoroutine(_attack);
    }

    public override void Freeze()
    {
        base.Freeze();

        // _isFreeze = true;
        _joystick.Deactivate();
    }
}
