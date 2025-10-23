using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
    [SerializeField] Slider rageSlider;
    public bool isOnRage;
    [SerializeField] float rageDecrease;

    public void Update()
    {
        /*
        if(isOnRage)
        {
            OnRage();
        }
        */
    }

    public void SetMaxValue(int newMaxValue)
    {
        rageSlider.maxValue = newMaxValue;
    }

    public void SetRage(float currentRage)
    {
        rageSlider.value = currentRage;
    }

    public void OnRage()
    {
        if (rageSlider.value > 0)
        {
            rageSlider.value -= rageDecrease * Time.deltaTime;
        }
        else
        {
            isOnRage = false;
        }
    }
}
