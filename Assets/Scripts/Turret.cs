using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Gun
{
    [SerializeField] private Transform _rotationTurret;
    private Quaternion _defaultTurretPosition;

    private void Awake()
    {
        _defaultTurretPosition = transform.rotation;
    }

    private void Update()
    {
        FindTarget();

        if (_target != null)
            RotationToTarget(_target.transform.position);
        else
            RotationDefaultPosition();
    }

    private void RotationToTarget(Vector3 position)
    {
        Vector3 direction = position - _rotationTurret.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        _rotationTurret.transform.DORotateQuaternion(new Quaternion(0, rotation.y, 0, rotation.w), 0.6f);
    }

    private void RotationDefaultPosition()
    {
        _rotationTurret.transform.DORotate(_defaultTurretPosition.eulerAngles, 0.6f);
    }
}
