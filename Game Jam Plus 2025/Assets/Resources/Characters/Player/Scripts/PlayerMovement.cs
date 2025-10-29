using UnityEngine;
using DG.Tweening;
using System.Collections;
// OBS: Every function with the "DO" prefix is a DOTween function.
// Although it isn't a official syntax, its used to identify those functions by their names.

public class PlayerMovement : MonoBehaviour
{
    private DialogueManager _dialogueManager;

    [Header("Other Scripts & Components References")]
    public Animator animator;

    [Header("Status")]
    [SerializeField] bool _isInvincible;
    [SerializeField] bool _isSuperArmorActive;

    [Header("Walk Config")]
    public bool canMove;
    [SerializeField] [Range(1, 20)] float _moveSpeed = 6;

    [Header("Dodge Config")]
    [SerializeField] float _dodgeForce = 50;
    [SerializeField] Vector2 lastVelocity;
    float bounceFactor = 1.2f;

    [Space(10)]
    public Transform orcVisual;
    private Rigidbody2D _rb;

    [Header("Shadow Config")]
    [SerializeField] GameObject _shadow;

    #region Particles Variables
    [SerializeField] protected ParticleSystem dustParticles;
    [SerializeField] protected Rigidbody2D rigidBody;
    protected Vector2 particleStartPos;
    #endregion

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public bool IsInvincible { get => _isInvincible; set => _isInvincible = value; }
    public bool IsSuperArmorActive { get => _isSuperArmorActive; set => _isSuperArmorActive = value; }
    public Vector2 LastVelocity { get => lastVelocity; set => lastVelocity = value; }
    public float BounceFactor { get => bounceFactor; set => bounceFactor = value; }

    void Start()
    {
        // This script setup
        canMove = true;

        // Finding references
        animator = GetComponent<Animator>();
        _dialogueManager = FindAnyObjectByType<DialogueManager>();

        _rb = GetComponent<Rigidbody2D>();

        particleStartPos = dustParticles.transform.localPosition;
    }

    void FixedUpdate()
    {
        StartDustParticles();

        LastVelocity = _rb.linearVelocity;

        if(canMove)
        {
            Move();
            if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                DodgeAttack();
                //Jump();
            }
        }
    }

    // Movement and jump functions
    private void Move()
    {
        Vector3 direction = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            MoveSpeed = 12f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeed = 6f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up;
        }
        if(Input.GetKey(KeyCode.S))
        {
            direction -= Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            orcVisual.rotation = Quaternion.Euler(0, 180f, 0);

            direction += Vector3.left;
            
            Vector2 particlePosition = particleStartPos;
            particlePosition.x *= -1;
            dustParticles.transform.localPosition = particlePosition;
            dustParticles.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        if (Input.GetKey(KeyCode.D))
        {
            orcVisual.rotation = Quaternion.Euler(0, 360f, 0);

            direction += Vector3.right;
            dustParticles.transform.localPosition = particleStartPos;
            dustParticles.transform.rotation = Quaternion.Euler(0,0,0); 
        }

        // Handle animations
        if(direction != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        transform.position += direction.normalized * MoveSpeed * Time.deltaTime;
    }

    private void ChangeMovement(float value)
    {
        MoveSpeed = value;
    }

    private void StartDustParticles()
    {
        if (animator.GetBool("isWalking"))
        {
            dustParticles.Play();
        }
        else
        {
            dustParticles.Stop();
        }
    }

    private void Jump()
    {
        // Activates jump Animation
        animator.SetTrigger("jump");

        // Original jump with DOTween
        //_orcVisual.DOLocalMove(new Vector3(0, _jumpHeight, 0), _jumpDuration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
    }

    private void DodgeAttack()
    {
        ActivateInvincibility();

        animator.SetTrigger("dodge attack");

        var direction = orcVisual.right;

        _rb.AddForce(direction * _dodgeForce, ForceMode2D.Impulse);

        var pAttack = GetComponent<PlayerAttacks1>();
        pAttack.CurrentThrowForce = 30f;
    }

    // Both called by animator's animation event
    public void ActivateInvincibility()
    {
        IsInvincible = true;
    }
    public void DeactivateInvincibility()
    {
        IsInvincible = false;
    }

    // Use these functions to make other scripts control temporarily the player's movement and jump respectively
    public void DOMoveSomewhere(Vector3 npcPos, Vector3 finalPos, float duration)
    {
        canMove = false;

        transform.DOLocalMove(finalPos, duration).OnComplete(() =>
        {
            var direction = orcVisual.position - npcPos;

            orcVisual.transform.DOLocalRotate(direction, 0.1f);
        });
    }


    // Implementar essa função para a animação de andar parar imediatamente
    public void StopWalk()
    {
        animator.SetBool("isWalking", false);
        animator.Play("Idle");

        var playerAttack1 = GetComponent<PlayerAttacks1>();

        playerAttack1.IsAnim = false;
    }

    
    // COROUTINES
    IEnumerator WaitPlaceholder(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
