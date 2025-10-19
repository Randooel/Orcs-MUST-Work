using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;

    public void SetMaxValue(int newMaxValue)
    {
        healthSlider.maxValue = newMaxValue;
    }

    public void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
