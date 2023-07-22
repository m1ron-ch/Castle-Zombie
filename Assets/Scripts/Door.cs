using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;

    private RoomManager _roomManager;
    private float _maxHealth = 100;
    private float _health;
    private bool _isClose = false;

    public bool IsClose => _isClose;

    private void Start()
    {
        _maxHealth = _health;
    }

    public void Init(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public void Health(float health)
    {
        if (_maxHealth > _health + health) _health = _maxHealth;
        else _health += health;
    }

    public void Close()
    {
        _isClose = true;

        transform.DOMoveY(0, 0.7f);
        _healthBar.Show();

        gameObject.SetActive(true);
        _roomManager.IsAllDoorsClose();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
