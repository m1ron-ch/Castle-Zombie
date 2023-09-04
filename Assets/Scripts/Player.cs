using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Player : Humanoid
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private Transform _dustPrefab;
    [SerializeField] private Transform _dustInstatiatePoint;

    [Header("Tools")]
    [SerializeField] private Gun _personalWeapon;
    [SerializeField] private Transform _axe;
    [SerializeField] private Transform _pickaxe;

    private enum Status
    {
        None, Mine, Attack, Death, Wait
    }

    private static Player s_instance;
    private Rigidbody _rb;
    private Animator _animator;
    private Transform _inHand;
    private GameObject _currentResource;
    private Coroutine _attack;
    private Key.Animations _currentAnimation;
    private Status _status = Status.None;
    private bool _isDust = false;

    [Header("Player Paramas")]
    private int _maxHealth;
    private int _health = 100;
    private int _damage = 10;
    private float _speed = 5;
    private bool _isGod;

    public static Player Instance => s_instance;
    public bool IsMove => _joystick.IsTouch;

    #region MonoBehaviour
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _maxHealth = _health;

        _personalWeapon.gameObject.SetActive(false);
        _axe.gameObject.SetActive(false);
        _pickaxe.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Debug.Log($"Status: {_status}");

        if ((_status == Status.Death) || (_status == Status.Wait))
            return;

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
        Enemy enemy = EnemyManager.Instance.GetNearestEnemy(transform.position);
        if (enemy != null)
        {
            HideAllTools();

            _status = Status.Attack;
            if (!_personalWeapon.gameObject.activeInHierarchy)
                Util.Invoke(this, () => ShowObject(_personalWeapon.transform), 0.3f);

            _currentAnimation = Key.Animations.PistolRunning;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), true);
            _animator.SetBool(Key.Animations.Idle.ToString(), false);
            _animator.SetBool(Key.Animations.Running.ToString(), false);
        }
        else
        {
            _status = _status != Status.Mine ? Status.None : Status.Mine;
            if (_personalWeapon.gameObject.activeInHierarchy)
                Util.Invoke(this, () => HideObject(_personalWeapon.transform), 0.7f);

            _currentAnimation = Key.Animations.Running;
            _animator.SetBool(Key.Animations.PistolIdle.ToString(), false);
            _animator.SetBool(Key.Animations.PistolRunning.ToString(), false);
            _animator.SetBool(Key.Animations.Idle.ToString(), true);
        }
        #endregion

        if (direction != Vector3.zero)
        {
            if (!_isDust)
                StartCoroutine(DustEmitter());
        }
        else
        {
            _isDust = false;
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        Vector3 relativePos = transform.position;
        relativePos.Set(_joystick.Horizontal, 0, _joystick.Vertical);

        if (enemy != null)
            rotation = Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles;
        else if (_status == Status.Mine)
            rotation = new Vector3 (0, Quaternion.LookRotation(_currentResource.transform.position - transform.position).eulerAngles.y, 0);
        else if (direction != Vector3.zero)
            rotation = Quaternion.LookRotation(relativePos).eulerAngles;

        transform.DORotate(rotation, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleObjectInteraction(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleObjectInteraction(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (ReferenceEquals(other.gameObject, _currentResource))
        {
            ExitFromAttackResource();
            _currentResource = null;
        }
    }
    #endregion

    public void Health(int value)
    {
        if (value + _health >= _maxHealth)
            _health = _maxHealth;
        else
            _health += value;

        _healthBar.Health(value, _maxHealth);
    }

    public void Damage(int value)
    {
        if (_isGod)
        {
            return;
        }

        _health -= value;
        _healthBar.Damage(value, _maxHealth);

        Death();
    }

    public void Revive()
    {
        _health = _maxHealth;
        Health(_maxHealth);

        _isGod = true;
        Util.Invoke(this, () => _isGod = false, 3f);

        _status = Status.None;
        _animator.SetBool(Key.Animations.Death.ToString(), false);

        EnemyManager.Instance.FindPlayerForAllEnemy();

        _personalWeapon.Active();
        UIRevive.Instance.Hide();
    }

    public void Wait(bool value)
    {
        _animator.SetBool(Key.Animations.Running.ToString(), false);    
        _status = value ? Status.Wait : Status.None;
    }

    private void HandleObjectInteraction(GameObject obj)
    {
        if (IsMove || (_status == Status.Attack) || (_currentResource != null))
        {
            return;
        }

        if (obj.TryGetComponent(out ObjectType objectType) && obj.TryGetComponent(out Resource resource))
        {
            _currentResource = obj;

            switch (objectType.Type)
            {
                case Key.ObjectType.Tree:
                    ShowObject(_axe);
                    break;
                case Key.ObjectType.Rock:
                    ShowObject(_pickaxe);
                    break;
                default:
                    break;
            }

            _attack = StartCoroutine(Attack(resource, objectType.Type));
        }
    }

    private IEnumerator Attack(Resource resource, Key.ObjectType type)
    {
        _status = Status.Mine;

        while (_status == Status.Mine)
        {
            if (IsMove || !resource.Damage(_damage))
            {
                ExitFromAttackResource();
                yield break;
            }

            switch (type)
            {
                case Key.ObjectType.Tree:
                    _animator.SetTrigger(Key.Animations.Chop.ToString());
                    SoundManager.Instance.PlayChop();
                    break;
                case Key.ObjectType.Rock:
                    _animator.SetTrigger(Key.Animations.Chop.ToString());
                    Util.Invoke(this, () => SoundManager.Instance.PlayPickRock(), 0.35f);
                    break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator DustEmitter()
    {
        _isDust = true;

        while (_isDust)
        {
            if (_status == Status.Death)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.1f);

            Vector3 playerPosition = _dustInstatiatePoint.position;
            float randomOffset = Random.Range(-0.5f, 0.5f);
            Vector3 spawnPosition = new Vector3(playerPosition.x + randomOffset, playerPosition.y, playerPosition.z);

            float scale = Random.Range(0.7f, 1.3f);
            Transform dust = Instantiate(_dustPrefab, spawnPosition, Quaternion.identity);
            dust.localScale *= scale;
            StartCoroutine(DestroyAfterDelay(dust.gameObject, 0.4f));
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        target.transform.DOScale(0, 0.6f).SetDelay(0.2f);
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }

    private void StopCoroutine()
    {
        if (_attack != null)
            StopCoroutine(_attack);
    }

    private void Death()
    {
        if (_health > 0)
            return;

        _status = Status.Death;
        _animator.SetBool(Key.Animations.Death.ToString(), true);

        EnemyManager.Instance.ResetTargetAllEnemy();
        UIRevive.Instance.Show();

        _personalWeapon.Deactivate();
    }

    private void ExitFromAttackResource()
    {
        StopCoroutine();
        StopAllCoroutines();

        _status = Status.None;
        _currentResource = null;

        _animator.ResetTrigger(Key.Animations.Chop.ToString());
        Util.Invoke(this, () => HideObject(_inHand), 1.2f);
    }

    private void HideAllTools()
    {
        _axe.gameObject.SetActive(false);
        _pickaxe.gameObject.SetActive(false);
    }

    private void ShowObject(Transform obj)
    {
        if (_inHand != null)
        {
            HideObject(_inHand);
        }

        _inHand = obj;
        obj.gameObject?.SetActive(true);
    }

    private void HideObject(Transform obj)
    {
        if (obj == null)
        {
            return;
        }

        obj.gameObject?.SetActive(false);
        _inHand = null;
    }
}
