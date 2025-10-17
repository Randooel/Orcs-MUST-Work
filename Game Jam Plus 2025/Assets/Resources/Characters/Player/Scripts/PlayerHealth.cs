using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Other Components References")]
    private Animator _animator;
    private PlayerMovement _playerMovement;

    [Header("Health Config")]
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth = 5;

    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    void Start()
    {
        // Set up
        RestoreHealth(_maxHealth);

        // Setting references up
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = _playerMovement.animator;
    }

    public void RestoreHealth(int heal)
    {
        if(heal >=  _maxHealth)
        {
            CurrentHealth = MaxHealth;
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
}
