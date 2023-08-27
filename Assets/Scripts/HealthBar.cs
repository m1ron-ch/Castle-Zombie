using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        SetMaxHealth(); 
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void SetMaxHealth()
    {
        _slider.value = 1;
    }

    public void Damage(int value, int maxHealth)
    {
        _slider.value -= Util.CalculatePercentage(value, maxHealth);
    }

    public void Health(int value, int maxHealth)
    {
        _slider.value += Util.CalculatePercentage(value, maxHealth);
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
