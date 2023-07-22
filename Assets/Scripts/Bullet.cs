using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage;

    public float Damage => _damage;

    public void SetDamage(float value)
    {
        _damage = value;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
