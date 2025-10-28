using UnityEngine;

using TMPro;
using DG.Tweening;
using System;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    [SerializeField] int _currentComboCounter;
    [SerializeField] float _timeToResetCounter;

    public static Action<bool> OnHit;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI _comboText;

    private void OnEnable()
    {
        OnHit += RefreshCombo;
    }

    private void OnDisable()
    {
        OnHit -= RefreshCombo;
    }

    void Start()
    {
        if(_comboText == null)
        {
            Debug.LogWarning("ComboText component not found!");
        }

        ResetComboCounter();
    }

    // COMBO COUNTER RELATED
    public void RefreshCombo(bool shouldIncrease)
    {
        StopCoroutine("WaitToResetCounter");
        DOTween.KillAll();

        DOResetTilt(0);

        if(shouldIncrease)
        {
            _currentComboCounter++;
            _comboText.text = _currentComboCounter.ToString() + " hits!";

            ApplyColor(ChooseRandomColor());

            DOTilt();
        }
        else
        {
            ResetComboCounter();
        }

        StartCoroutine(WaitToResetCounter(3f));
    }

    private void ResetComboCounter()
    {
        _currentComboCounter = 0;
        _comboText.text = "";

        ResetColor();
    }

    // ANIMATION
    private void DOTilt()
    {
        _comboText.gameObject.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            DOResetTilt(0.25f);
        });
    }

    private void DOResetTilt(float duration)
    {
        _comboText.gameObject.transform.DOScale(1f, duration);
    }

    // COLOR
    private Color ChooseRandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    private void ApplyColor(Color color)
    {
        _comboText.color = color;
    }

    private void ResetColor()
    {
        _comboText.color = Color.white;
    }

    // COROUTINES
    IEnumerator WaitToResetCounter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ResetComboCounter();
    }
}
