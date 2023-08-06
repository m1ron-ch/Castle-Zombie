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
        _chopPositionY -= _choping;
        transform.DOShakeRotation(0.7f, 10, 5, 10)
            .SetDelay(0.5f)
            .OnStart(() => {
                // transform.DOMoveY(_chopPositionY, 0.2f);
                AddResource();
            })
            .OnComplete(() => Death());

        _health -= value;

        return _health > 0;
    }

    private void AddResource()
    {
        switch (_objectType.Type)
        {
            case Key.ObjectType.Tree:
                ResourceController.AddResource(Key.Prefs.Wood, 10);
                break;
            case Key.ObjectType.Rock:
                ResourceController.AddResource(Key.Prefs.Rock, 5);
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
