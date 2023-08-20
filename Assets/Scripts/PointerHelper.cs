using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class PointerHelper : MonoBehaviour
{
    public Transform _player;
    public GameObject _pointer;

    private static PointerHelper s_instance;
    private Transform _target;
    private Camera _mainCamera;
    private float _rotationSpeed = 10f;
    private float _circleRadius = 1.5f;
    private bool _isShowPointer;

    public static PointerHelper Instance => s_instance;

    #region MonoBehaviour
    private void Awake()
    {
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
        _mainCamera = Camera.main;

        _pointer.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_target == null || _isShowPointer)
            return;

        Vector3 targetScreenPosition = _mainCamera.WorldToViewportPoint(_target.position);

        if (targetScreenPosition.x < 0 || targetScreenPosition.x > 1 ||
            targetScreenPosition.y < 0 || targetScreenPosition.y > 1 ||
            targetScreenPosition.z < 0)
        {
            Vector3 targetDirection = (_target.position - _player.position).normalized;

            Vector3 circlePosition = _player.position + targetDirection * _circleRadius;
            _pointer.transform.position = circlePosition + Vector3.up * 1;

            Quaternion targetRotation = Quaternion.LookRotation(_target.position - circlePosition, Vector3.up) * Quaternion.Euler(-90, 0, 0);
            _pointer.transform.rotation = Quaternion.Slerp(_pointer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _pointer.transform.position = new Vector3(_target.position.x, 4, _target.position.z);
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            _pointer.transform.rotation = Quaternion.Slerp(_pointer.transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);


        }
    }
    #endregion

    public void SetTarget(Transform target)
    {
        _target = target;
       _pointer.SetActive(true);
    }

    public void ResetTarget()
    {
        _isShowPointer = true;
        _target = null;

        _pointer.transform.position = -Vector3.one;
        _pointer.gameObject.SetActive(false);
    }
}
