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
    private float _speed = 8;
    private bool _isFreeze;

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

    public override void Freeze()
    {
        base.Freeze();

        _isFreeze = true;
        _joystick.Deactivate();

        if (Camera.main.TryGetComponent(out FollowingCamera camera))
        {
            // camera.IsFollowing(false);
            // camera.transform.DOMove(camera.transform.position + Vector3.forward * 2.5f, 0.5f);
        }
    }
}
