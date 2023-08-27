using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private static FollowingCamera s_instance;
    private Vector3 _offset;
    private float _transitionDuration = 2.0f;
    private bool _isFollowing = true;

    public static FollowingCamera Instance => s_instance;

    private void Awake()
    {
        _offset = Camera.main.transform.position - _player.transform.position;

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

    private void FixedUpdate()
    {
        if (_isFollowing)
            transform.position = Vector3.Slerp(transform.position, _player.position + _offset, 0.2f);
    }

    public void MoveToBuildings(List<BuildingPoint> buildings)
    {
        _isFollowing = false;
        Player.Instance.Wait(true);

        Sequence cameraSequence = DOTween.Sequence();

        foreach (BuildingPoint building in buildings)
        {
            cameraSequence.Append(transform.DOMove(building.transform.position + _offset, _transitionDuration));
            // cameraSequence.AppendInterval(2.0f);
        }

        cameraSequence.OnComplete(() => 
        {
            _isFollowing = true;
            Player.Instance.Wait(false);
        });
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
