using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ObjectType))]
public class Resource : MonoBehaviour
{
    private ObjectType _objectType;
    private int _health = 50;
    private Vector3 _defaultRotation;
    private Collider _collider;
    float _chopPositionY = 0;
    float _choping;

    private void Start()
    {
        _objectType = GetComponent<ObjectType>();
        _defaultRotation = transform.rotation.eulerAngles;
        _collider = GetComponent<Collider>();

        _choping = _collider.bounds.extents.z / 2f;
    }

    public bool Damage(int value)
    {
        float delay = 0.5f;
        _chopPositionY -= _choping;
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

        return _health > 0;
    }

    private void AddResource()
    {
        switch (_objectType.Type)
        {
            case Key.ObjectType.Tree:
                ResourceController.AddResource(Key.ResourcePrefs.Wood, 10);
                break;
            case Key.ObjectType.Rock:
                ResourceController.AddResource(Key.ResourcePrefs.Rock, 5);
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
            .OnComplete(() => transform.gameObject.SetActive(false));
    }

}
