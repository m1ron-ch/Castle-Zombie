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
        Debug.Log(_choping + " " + gameObject.name);
    }

    public bool Damage(int value)
    {
        _chopPositionY -= _choping;
        transform.DOShakeRotation(0.7f, 10, 5, 10)
            .SetDelay(0.3f)
            .OnStart(() => {
                transform.DOMoveY(_chopPositionY, 0.2f);
                AddResource();
            })
            .OnComplete(() => Death());

        _health -= value;

        return _health > 0;
    }

    private void AddResource()
    {
        switch (_objectType.ObjectT)
        {
            case ObjectType.ObjectTypes.Tree:
                ResourceController.AddResource(Key.Prefs.Wood, 10);
                break;
            case ObjectType.ObjectTypes.Rock:
                ResourceController.AddResource(Key.Prefs.Rock, 5);
                break;
        }
    }

    private void Death()
    {
        if (_health > 0)
            return;

        transform.DOMoveY(-7, 2).SetDelay(0.5f)
            .OnStart(() => _collider.isTrigger = true)
            .OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
            });

        /*float rotation = GameObject.FindGameObjectWithTag("Player").transform.rotation.eulerAngles.y;*/
        /*        if (_objectType.ObjectT == ObjectType.ObjectTypes.Tree)
                    transform.DORotate(new Vector3(0, 180, 0), 2)
                        .OnStart(() => {  })
                        .OnComplete(() => { transform.gameObject.active = false; });*/
    }

}
