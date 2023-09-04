using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ObjectType))]
public class Resource : MonoBehaviour
{
    private ObjectType _objectType;
    private int _health = 50;
    private Collider _collider;
    private int _defaultHealth;

    private void Awake()
    {
        _objectType = GetComponent<ObjectType>();
        _collider = GetComponent<Collider>();

        _defaultHealth = _health;
    }

    public bool Damage(int value)
    {
        if (_health <= 0)
        {
            _collider.enabled = false;
            return false;
        }

        float delay = 0.5f;
        transform.DOShakeRotation(0.7f, 5, 1, 5)
            .SetDelay(delay)
            .OnStart(() => AddResource())
            .OnComplete(() => Death());

        transform.DOScale(0.85f, 0.15f)
            .SetEase(Ease.OutCubic)
            .SetDelay(delay)
            .OnComplete(() => transform.DOScale(1, 0.1f)
                                        .SetEase(Ease.Linear));

        _health -= value;
        return true;
    }

    private void AddResource()
    {
        switch (_objectType.Type)
        {
            case Key.ObjectType.Tree:
                ResourceController.AddResource(Key.ResourcePrefs.Wood, ResourceReward.Instance.WoodReward);
                break;
            case Key.ObjectType.Rock:
                ResourceController.AddResource(Key.ResourcePrefs.Rock, ResourceReward.Instance.RockReward);
                break;
        }
    }

    private void Death()
    {
        if (_health > 0)
            return;

        transform.DOMoveY(-7, 2)
            .SetDelay(0.5f)
            .OnStart(() => _collider.enabled = false)
            .OnComplete(() => Util.Invoke(this, () => Restart(), 1));
    }

    private void Restart()
    {
        _collider.enabled = true;
        transform.gameObject.SetActive(true);
        _health = _defaultHealth;

        transform.DOMoveY(0, 1.7f);
    }
}
