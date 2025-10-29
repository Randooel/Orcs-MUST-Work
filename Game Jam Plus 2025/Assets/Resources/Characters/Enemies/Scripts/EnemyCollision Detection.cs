using System.Collections;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour
{
    /*
    [Header("Detection Layer")]
    [SerializeField] protected LayerMask _detectionLayer;
    */

    public bool isAttackCollider;

    [Space(5)]
    private  EnemyBehavior _enemyBehavior;
    public bool collisionDetected;

    private void Start()
    {
        _enemyBehavior = GetComponentInParent<EnemyBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collisionDetected = true;

            var pMove = collision.GetComponent<PlayerMovement>();
            var isInvincible = pMove.IsInvincible;

            if (!isAttackCollider)
            {
                ActivateState();
            }
            else if(!isInvincible)
            {
                var playerHealth = collision.GetComponent<PlayerHealth>();

                playerHealth.PlayVFX();
                playerHealth.TakeDamage(_enemyBehavior.Damage1);
            }
        }
    }

    private void ActivateState()
    {
        _enemyBehavior.ToggleState();

        collisionDetected = false;
    }

    /*
    public void Raycast()
    {
        var raycast = Physics2D.Raycast(transform.up, transform.forward, _detectionLayer);
    }
    */
}
