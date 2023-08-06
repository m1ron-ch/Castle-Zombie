using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private Vector3 _offset;
    private bool _isFollowing = true;

    public Vector3 Offset => _offset;

    private void Awake()
    {
        _offset = Camera.main.transform.position - _player.transform.position;
    }

    private void FixedUpdate()
    {
        return;
        if (_isFollowing)
            transform.position = Vector3.Slerp(transform.position, _player.position + _offset, 0.2f);
    }

    public void Following()
    {
        if (_isFollowing)
            transform.position = Vector3.Slerp(transform.position, _player.position + _offset, 0.2f);
    }

    public void RotationTo(Vector3 rotation)
    {
        _isFollowing = false;
        transform.RotateAround(_player.position, Vector3.up, rotation.y);
    }

    public void IsFollowing(bool isFollowing)
    {
        _isFollowing = isFollowing;
    }
}
