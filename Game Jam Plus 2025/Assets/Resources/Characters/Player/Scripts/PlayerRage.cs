using System;
using UnityEngine;

using DG.Tweening;

public class PlayerRage : MonoBehaviour
{
    [Header("Rage Stats")]
    [SerializeField] float punchMultiplier;
    [SerializeField] float punchSpeed;
    [Space(5)]
    [SerializeField] float throwForceMultiplier;
    [SerializeField] Vector2 _sizeDuringRage = new Vector2(1.25f, 1.25f);

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
        // Runs timer to decrease current rage
        if(isOnRage)
        {
            RageTimer();
        }
    }

    // This time is meant to control the duration of the rage mode
    private void RageTimer()
    {
        // To increase or decrease the rage mode time, just change the number in the code down bellow
        CurrentRage -= 2 * Time.deltaTime;
        _rageBar.SetRage(CurrentRage);

        // Deactivates rage if it is equal or below 0
        if (CurrentRage <= 0)
        {
            DeactivateRage();
        }
    }

    // Updates the Rage Bar's value and, if it is enough to activate rage, it activates it
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

        // Activates super armor
        var pMove = GetComponent<PlayerMovement>();
        pMove.IsSuperArmorActive = true;

        // Scale player up
        transform.DOScale(_sizeDuringRage, 0.25f).SetEase(Ease.OutSine);

        // Update rage and rage bar
        CurrentRage = _maxRage;
        _rageBar.SetRage(CurrentRage);
    }

    public void DeactivateRage()
    {
        isOnRage = false;

        // Deactivates super armor
        var pMove = GetComponent<PlayerMovement>();
        pMove.IsSuperArmorActive = true;

        // Scale player down
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutSine);

        // Reset rage and rage bar
        CurrentRage = 0;
        _rageBar.SetRage(CurrentRage);
    }
}
