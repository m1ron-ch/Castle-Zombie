using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMPro.TMP_Text _healthText;

    private float _maxHealth;

    public void SetMaxHealth(float maxHealth)
    {
        _slider.value = 1;
        _healthText.text = maxHealth.ToString();

        _maxHealth = maxHealth;
    }

    public void SetHealth(float damage, float health)
    {
        _slider.value -= ((float)damage / Mathf.Pow(10, _maxHealth.ToString().Length - 1)) / 2;
        _healthText.text = health.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject?.SetActive(false);
    }
}
