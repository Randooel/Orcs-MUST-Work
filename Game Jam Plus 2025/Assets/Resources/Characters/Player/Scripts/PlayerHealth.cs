using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Other Components References")]
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerRage _playerRage;

    [Header("Health Config")]
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth = 5;

    [Header("UI Bars")]
    [SerializeField] HealthBar _healthBar;


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
        _playerRage = GetComponent<PlayerRage>();
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
            _healthBar.SetHealth(CurrentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!_playerMovement.IsSuperArmorActive)
        {
            _animator.SetTrigger("takeDamage");
        }

        _playerMovement.canMove = false;

        CurrentHealth -= damage;
        _healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Death();
            CurrentHealth = 0;
        }

        // Decrease Combo
        ComboManager.OnHit?.Invoke(false);

        _playerRage.RefreshRage(-damage);
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

    // Called by an event in the "Death" animation clip
    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
