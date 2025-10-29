using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerAttacks1 : MonoBehaviour
{
    private DialogueManager _dialogueManger;
    private PlayerRage _playerRage;
    
    [Header("Other Scripts & Components References")]
    private PlayerMovement _playerMovement;

    [Header("Combo config")]
    [SerializeField] int _currentDamage = 0;
    [SerializeField] float _currentThrowForce = 0;
    [SerializeField] int _hitCounter;
    [SerializeField] float _resetCounterTime = 1.5f;
    [SerializeField] bool _isAnim;
    private Coroutine _currentCoroutine;


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

    private void Attack()
    {
        _playerMovement.canMove = false;

        if (_hitCounter == 0)
        {
            _hitCounter++;
            CurrentThrowForce = 0f;

            StopCoroutine(WaitToResetHitCounter());

            //transform.DOMoveX(transform.position.x + 1.5f, 0.5f);

            _playerMovement.animator.SetTrigger("quick attack 0");
        }
        else if (_hitCounter == 1)
        {
            _hitCounter++;
            CurrentThrowForce = 0f;

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 1");
        }
        else if (_hitCounter == 2)
        {
            _hitCounter++;
            CurrentThrowForce =  0 /*50f*/;

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 2");
        }

        IsAnim = true;

        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // RAGE
        if (_playerRage.isOnRage)
        {
            CurrentDamage ++;
            CurrentThrowForce *= 2;
        }

        CurrentDamage++;
        //CurrentThrowForce++;
        _currentCoroutine = StartCoroutine(WaitToResetHitCounter());

        ResetHitCounter(3);
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