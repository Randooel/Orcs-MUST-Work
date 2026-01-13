using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PlayerAttacks1 : MonoBehaviour
{
    private DialogueManager _dialogueManger;
    private PlayerRage _playerRage;
    
    [Title("Other Scripts & Components References")]
    private PlayerMovement _playerMovement;

    [Title("Combo config")]
    [SerializeField] int _currentDamage = 0;
    [SerializeField] float _currentThrowForce = 0;
    [SerializeField] int _hitCounter;
    [SerializeField] float _resetCounterTime = 1.5f;
    [SerializeField] bool _isAnim;
    private Coroutine _currentCoroutine;

    [Title("Movement during attacks")]
    [SerializeField] private bool _isAttacking;
    [SerializeField] private float dashForce;

    [SerializeField] [Range(0, 50)] private List<float> _attackDashForce = new List<float>(4);


    public int CurrentDamage { get => _currentDamage; set => _currentDamage = value; }
    public float CurrentThrowForce { get => _currentThrowForce; set => _currentThrowForce = value; }
    public bool IsAnim { get => _isAnim; set => _isAnim = value; }

    void Start()
    {
        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        
        _playerRage = GetComponent<PlayerRage>();

        _dialogueManger = FindAnyObjectByType<DialogueManager>();
    }

    void Update()
    {
        if (_playerMovement.canMove && !IsAnim && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if(_isAttacking == true)
        {
            MoveDuringAttack(dashForce);
        }
    }

    private void Attack()
    {
        _playerMovement.canMove = false;

        if (_hitCounter == 0)
        {
            _hitCounter++;
            CurrentThrowForce = _attackDashForce[0];

            dashForce = _attackDashForce[0];

            StopCoroutine(WaitToResetHitCounter());

            //transform.DOMoveX(transform.position.x + 1.5f, 0.5f);

            _playerMovement.animator.SetTrigger("quick attack 0");
        }
        else if (_hitCounter == 1)
        {
            _hitCounter++;
            CurrentThrowForce = _attackDashForce[1];

            dashForce = _attackDashForce[1];

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 1");
        }
        else if (_hitCounter == 2)
        {
            _hitCounter++;
            CurrentThrowForce = 5f;

            CurrentThrowForce = _attackDashForce[2];

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 2");
        }
        else if (_hitCounter == 3)
        {
            _hitCounter++;
            CurrentThrowForce = 50f;

            dashForce = _attackDashForce[3];

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 3");
        }

        IsAnim = true;

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        CurrentDamage++;

        // RAGE
        if (_playerRage.isOnRage)
        {
            CurrentDamage *= 2;
            CurrentThrowForce *= 2;
        }

        _isAttacking = true;

        //CurrentThrowForce++;
        _currentCoroutine = StartCoroutine(WaitToResetHitCounter());

        ResetHitCounter(4);
    }

    public void AttackSomething()
    {
        _playerMovement.canMove = false;
    }

    public void ToggleIsAnim()
    {
        IsAnim = false;
    }

    public void ActivateCanMove()
    {
        if(_dialogueManger.isOnDialogue)
        {
            return;
        }
        else
        {
            _playerMovement.canMove = true;
        }
    }

    public void MoveDuringAttack(float dashForce)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        var direction = GetComponent<PlayerMovement>().orcVisual.right;
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);

        _isAttacking = false;
    }

    private void ResetHitCounter(int maxCount)
    {
        if (_hitCounter >= maxCount)
        {
            _hitCounter = 0;
            CurrentDamage = 1;
        }
    }

    // COROUTINES
    IEnumerator WaitToResetHitCounter()
    {
        yield return new WaitForSeconds(_resetCounterTime);

        ResetHitCounter(0);

        _currentCoroutine = null;
    }
}