using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpType _powerUpType;
    [SerializeField] float _powerValue;
    
    [Space(10)]
    [SerializeField] GameObject _currentPlayer;

    enum PowerUpType
    {
        None,
        Speed,
        Damage,
        Throw,
        Everything
    }

    void Start()
    {
        this.gameObject.name = _powerUpType.ToString() + "power up";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _currentPlayer = collision.gameObject;
            ActivatePowerUp();
        }
    }

    private void ActivatePowerUp()
    {
        if(_powerUpType != PowerUpType.None)
        {
            if(_powerUpType == PowerUpType.Speed)
            {
                _currentPlayer.GetComponent<PlayerMovement>().MoveSpeed *= _powerValue;
            }
            if(_powerUpType ==PowerUpType.Damage)
            {
                _currentPlayer.GetComponent<PlayerAttacks>().CurrentDamage++;
            }
        }
        else
        {
            Debug.LogWarning("This power up type was 'None'!");
        }
    }
}
