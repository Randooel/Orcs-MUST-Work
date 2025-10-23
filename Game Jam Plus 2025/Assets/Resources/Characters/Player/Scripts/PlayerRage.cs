using System;
using UnityEngine;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage Stats")]
    [SerializeField] float punchMultiplier;
    [SerializeField] float punchSpeed;
    [Space(5)]
    [SerializeField] float throwForceMultiplier;

    [Header("Rage Bar Config")]
    public bool isOnRage;
    [SerializeField] float _currentRage;
    [SerializeField] int _maxRage = 5;
    // [SerializeField] float rageTime = 5f;

    [Space(10)]
    [SerializeField] RageBar _rageBar;

    public float CurrentRage { get => _currentRage; set => _currentRage = value; }

    private void Start()
    {
        if(_rageBar != null)
        {
            _rageBar.SetMaxValue(_maxRage);
            _rageBar.SetRage(CurrentRage);
        }
        else
        {
            Debug.Log("RageBar was not found!");
        }
    }

    void Update()
    {
        if(isOnRage)
        {
            RageTimer();
        }
    }

    private void RageTimer()
    {
        CurrentRage -= 1 * Time.deltaTime;
        _rageBar.SetRage(CurrentRage);

        if (CurrentRage <= 0)
        {
            DeactivateRage();
        }
    }

    public void RefreshRage(float rageValue)
    {
        CurrentRage += rageValue;
        _rageBar.SetRage(CurrentRage);

        if(CurrentRage >= _maxRage)
        {
            ActivateRage();
        }
    }

    public void ActivateRage()
    {
        isOnRage = true;

        CurrentRage = _maxRage;
        _rageBar.SetRage(CurrentRage);
    }

    public void DeactivateRage()
    {
        isOnRage = false;

        CurrentRage = 0;
        _rageBar.SetRage(CurrentRage);
    }
}
