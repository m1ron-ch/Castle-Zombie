using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Player : Humanoid
{
    [SerializeField] private DynamicJoystick _joystick;

    private Rigidbody _rb;
    private Animator _animator;
    private Transform _resource;
    private Key.Animations _currentAnimation;
    private Coroutine _attack;
    private float _speed = 5;
    private int _damage = 10;
    private bool _isFreeze;
    private bool _isAttackResource;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_isFreeze)
        {
            _animator.SetBool(Key.Animations.Running.ToString(), false);
            return;
        }

        _animator.SetBool(Key.Animations.Running.ToString(), _joystick.IsTouch);

        Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;

        _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Vector3 relativePos = transform.position;
            relativePos.Set(_joystick.Horizontal, 0, _joystick.Vertical);
            transform.rotation = Quaternion.LookRotation(relativePos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ObjectType objectType))
        {
            switch (objectType.ObjectT)
            {
                case ObjectType.ObjectTypes.Tree:
                case ObjectType.ObjectTypes.Rock:
                    if (_resource == other.gameObject.transform)
                        return;

                    if (other.gameObject.TryGetComponent(out Resource resource))
                    {
                        _isAttackResource = true;
                        _resource = other.gameObject.transform;
                        _currentAnimation = Key.Animations.Chop;

                        _attack = StartCoroutine(Attack(resource));
                    }
                    break;
                case ObjectType.ObjectTypes.MachineGun:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _resource = null;
        StopCoroutine();
        _animator.ResetTrigger(_currentAnimation.ToString());

        if (_resource == other.gameObject.transform)
        {
        }
    }

    private IEnumerator Attack(Resource resource)
    {
        while (_isAttackResource)
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
        _isAttackResource = false;
       StopCoroutine(_attack);
    }

    public override void Freeze()
    {
        base.Freeze();

        _isFreeze = true;
        _joystick.Deactivate();
    }
}
