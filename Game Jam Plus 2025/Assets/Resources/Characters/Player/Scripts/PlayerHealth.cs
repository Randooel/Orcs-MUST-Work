using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Other Components References")]
    private Animator _animator;
    private PlayerMovement _playerMovement;

    [Header("Health Config")]
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth = 5;

    [Header("UI Bars")]
    [SerializeField] HealthBar _healthBar;
    [SerializeField] RageBar _rageBar;


    [Header("VFX")]
    #region VFX
    [SerializeField] protected GameObject hit;
    #endregion

    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    void Start()
    {
        // Set up
        _healthBar.SetMaxValue(_maxHealth);
        RestoreHealth(_maxHealth);

        hit.gameObject.SetActive(false);

        // Setting references up
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = _playerMovement.animator;
    }

    public void RestoreHealth(int heal)
    {
        if(heal >=  _maxHealth)
        {
            CurrentHealth = MaxHealth;
            _healthBar.SetHealth(_maxHealth);
        }
        else
        {
            CurrentHealth += heal;
        }
    }

    public void TakeDamage(int damage)
    {
        _animator.SetTrigger("takeDamage");

        _playerMovement.canMove = false;

        CurrentHealth -= damage;
        _healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        _animator.SetTrigger("death");

        //_gameManager.RebootGame();
    }

    // VFX ATTACKS
    public void PlayVFX()
    {
        hit.gameObject.SetActive(true);
        //hit.gameObject.GetComponentInChildren<VisualEffect>().Play();

        DOVirtual.DelayedCall(0.1f, () =>
        {
            hit.gameObject.SetActive(false);
        });
    }
}
