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
    [SerializeField] private UIFloatingText _text;

    [Header("Tools")]
    [SerializeField] private Transform _personalWeapon;
    [SerializeField] private Transform _axe;

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

    public bool IsMove => _joystick.IsTouch;

    #region MonoBehaviour
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _personalWeapon.gameObject.SetActive(false);
        _axe.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Debug.Log($"Status: {_status}");

        _animator.SetBool(_currentAnimation.ToString(), _joystick.IsTouch);
        if (_joystick.IsTouch)
        {
            SoundManager.Instance.PlayRunning();
        }
        else
        {
            SoundManager.Instance.StopRunning();
        }

        Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;
        _rb.MovePosition(transform.position + direction * _speed * Time.fixedDeltaTime);

        #region переделать
        Enemy enemy = _enemyManager.GetNearestEnemy(transform.position);
        if (enemy != null)
        {
            _status = Status.Attack;
            if (!_personalWeapon.gameObject.activeInHierarchy)
                Util.Invoke(this, () => _personalWeapon.gameObject.SetActive(true), 0.3f);

            _currentAnimation = Key.Animations.PistolRunning;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), true);
            _animator.SetBool(Key.Animations.Idle.ToString(), false);
            _animator.SetBool(Key.Animations.Running.ToString(), false);
        }
        else
        {
            _status = _status != Status.Mine ? Status.None : Status.Mine;
            if (_personalWeapon.gameObject.activeInHierarchy)
                Util.Invoke(this, () => _personalWeapon.gameObject.SetActive(false), 0.7f);

            _currentAnimation = Key.Animations.Running;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), false);
            _animator.SetBool(Key.Animations.PistolRunning.ToString(), false);
            _animator.SetBool(Key.Animations.Idle.ToString(), true);
        }
        #endregion

        Vector3 rotation = transform.rotation.eulerAngles;
        Vector3 relativePos = transform.position;
        relativePos.Set(_joystick.Horizontal, 0, _joystick.Vertical);

        if (enemy != null)
            rotation = Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles;
        else if (_status == Status.Mine)
            rotation = new Vector3 (0, Quaternion.LookRotation(_resource.position - transform.position).eulerAngles.y, 0);
        else if (direction != Vector3.zero)
            rotation = Quaternion.LookRotation(relativePos).eulerAngles;

        transform.DORotate(rotation, 0.2f);
        // transform.DORotate(enemy != null ? Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles : Quaternion.LookRotation(relativePos).eulerAngles, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleObjectInteraction(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ObjectType objectType))
        {
            ExitFromAttackResource();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        HandleObjectInteraction(other.gameObject);
    }
    #endregion

    private void HandleObjectInteraction(GameObject other)
    {
        if (IsMove || (_status == Status.Attack) || (_resource != null))
            return;

        if (other.gameObject.TryGetComponent(out ObjectType objectType))
        {
            switch (objectType.Type)
            {
                case Key.ObjectType.Tree:
                case Key.ObjectType.Rock:
                    if (other.TryGetComponent(out Resource resource))
                    {
                        _status = Status.Mine;
                        _resource = other.transform;
                        ShowObject(_axe);

                        _attack = StartCoroutine(Attack(resource));
                    }

                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator Attack(Resource resource)
    {
        while (true)
        {
            if (!resource.Damage(_damage) || IsMove)
            {
                ExitFromAttackResource();
                yield break;
            }

            _animator.SetTrigger(Key.Animations.Chop.ToString());
            SoundManager.Instance.PlayChop();

            yield return new WaitForSeconds(1f);
        }
    }


    private void StopCoroutine()
    {
        if (_attack != null)
            StopCoroutine(_attack);
    }

    private void ExitFromAttackResource()
    {
        StopCoroutine();

        _status = Status.None;
        _resource = null;

        Util.Invoke(this, () => HideObject(_axe), 1.7f);
        if (_currentAnimation == Key.Animations.Chop)
            _animator.ResetTrigger(_currentAnimation.ToString());
    }

    private void ShowObject(Transform obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void HideObject(Transform obj)
    {
        obj.gameObject.SetActive(false);
    }

    public override void Freeze()
    {
        base.Freeze();

        // _isFreeze = true;
        _joystick.Deactivate();
    }
}
