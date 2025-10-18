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

    [Header("Walk Config")]
    public bool canMove;
    [SerializeField] [Range(1, 10)] float _moveSpeed = 6;

    [Header("Jump is Handled as a animation")]
    [SerializeField] Transform _orcVisual;

    [Header("Shadow Config")]
    [SerializeField] GameObject _shadow;

    void Start()
    {
        // This script setup
        canMove = true;

        // Finding references
        animator = GetComponent<Animator>();
        _dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            Move();
            if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    // Movement and jump functions
    private void Move()
    {
        Vector3 direction = Vector3.zero;

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
            _orcVisual.rotation = Quaternion.Euler(0, 180f, 0);

            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _orcVisual.rotation = Quaternion.Euler(0, 360f, 0);

            direction += Vector3.right;
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
        
        transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
    }

    private void ChangeMovement(float value)
    {
        _moveSpeed = value;
    }

    private void Jump()
    {
        // Activates jump Animation
        animator.SetTrigger("jump");

        // Original jump with DOTween
        //_orcVisual.DOLocalMove(new Vector3(0, _jumpHeight, 0), _jumpDuration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutSine);
    }

    // Use these functions to make other scripts control temporarily the player's movement and jump respectively
    public void DOMoveSomewhere(Vector3 finalPos, float duration)
    {
        canMove = false;

        transform.DOLocalMove(finalPos, duration).OnComplete(() =>
        {
            canMove = true;
        });
    }


    // Implementar essa função para a animação de andar parar imediatamente
    public void StopWalk()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Walk"))
        {
            animator.Play("Idle");
        }
    }

    
    // COROUTINES
    IEnumerator WaitPlaceholder(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
