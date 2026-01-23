using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
#if UNITY_EDITOR
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
#endif
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PlayerAttacks : MonoBehaviour
{
    [Title("Other Scripts & Components References")]
    private PlayerMovement _playerMovement;
    private DialogueManager _dialogueManger;
    private PlayerRage _playerRage;

    #region Combo Config TabGroup
    [TabGroup("1", "Combo config")]
    [SerializeField] int _currentDamage = 0;

    [TabGroup("1", "Combo config")]
    [SerializeField] float _currentThrowForce = 0;

    [TabGroup("1", "Combo config")]
    [SerializeField] int _quickHitCounter;

    [TabGroup("1", "Combo config")]
    [SerializeField] int _strongHitCounter;

    [TabGroup("1", "Combo config")]
    [SerializeField] float _resetCounterTime = 1.5f;

    [TabGroup("1", "Combo config")]
    [SerializeField] bool _isAnim;

    [TabGroup("1", "Combo config")]
    private Coroutine _currentCoroutine;
    #endregion

    #region Attack Stats Modifiers
    [TabGroup("2", "Quick Attacks Modifiers")] [SerializeField] AttackModifier _quickAttack1;
    [TabGroup("2", "Quick Attacks Modifiers")] [SerializeField] AttackModifier _quickAttack2;

    [TabGroup("2", "Strong Attacks Modifiers")] [SerializeField] AttackModifier _strongAttack1;
    [TabGroup("2", "Strong Attacks Modifiers")] [SerializeField] AttackModifier _strongAttack2;
    #endregion

    #region Movement During Attacks Config TabGroup
    [TabGroup("3", "Movement During Attacks")]
    [SerializeField] private bool _isAttacking;

    [TabGroup("3", "Movement During Attacks")]
    [SerializeField] private float dashForce;

    [TabGroup("3", "Movement During Attacks")]
    [SerializeField][Range(0, 100)] private List<float> _attackDashForce = new List<float>(4);
    #endregion

    #region Encapsulated variables
    public int CurrentDamage { get => _currentDamage; set => _currentDamage = value; }
    public float CurrentThrowForce { get => _currentThrowForce; set => _currentThrowForce = value; }
    public bool IsAnim { get => _isAnim; set => _isAnim = value; }
    #endregion

    

    #region Start, Update and FixedUpdate
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
            //Attack();
            QuickAttack();
        }
        if(_playerMovement.canMove && !IsAnim && Input.GetMouseButtonDown(1))
        {
            StrongAttack();
        }

        // SCRIPT TO TEST ONE SPECIFC ATTACK AT A TIME
        //_quickHitCounter = 0;
    }

    void FixedUpdate()
    {
        if(_isAttacking == true)
        {
            MoveDuringAttack(dashForce);
        }
    }
    #endregion



    private void QuickAttack()
    {
        if (_quickHitCounter == 0)
        {
            _quickHitCounter++;

            // Setting Damage and ThrowForce
            CurrentDamage = _quickAttack1.newDamage;
            CurrentThrowForce = _quickAttack1.newThrowForce /* _attackDashForce[4] + */ ;

            // Setting dash force
            dashForce = _attackDashForce[0];

            StopCoroutine(WaitToResetHitCounter());
            _playerMovement.animator.SetTrigger("quick attack 1");
        }
        else if (_quickHitCounter == 1)
        {
            _quickHitCounter++;
            CurrentThrowForce = _quickAttack2.newThrowForce;

            dashForce = _attackDashForce[1];

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("quick attack 2");

            _quickHitCounter = 0;
        }

        HandleAttack();
    }

    private void StrongAttack()
    {
        if (_strongHitCounter == 0)
        {
            _strongHitCounter++;

            // Setting Damage and ThrowForce
            CurrentDamage = _strongAttack1.newDamage;
            CurrentThrowForce = _strongAttack1.newThrowForce /* _attackDashForce[4] + */ ;

            // Setting dash force
            dashForce = _attackDashForce[2];

            StopCoroutine(WaitToResetHitCounter());
            _playerMovement.animator.SetTrigger("strong attack 1");
        }
        else if (_strongHitCounter == 1)
        {
            _strongHitCounter++;
            CurrentDamage = _strongAttack2.newDamage;
            CurrentThrowForce = _strongAttack2.newThrowForce;

            dashForce = _attackDashForce[3];

            StopCoroutine(WaitToResetHitCounter());

            _playerMovement.animator.SetTrigger("strong attack 2");

            _strongHitCounter = 0;
        }

        HandleAttack();
    }

    private void HandleAttack()
    {
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
        if (_quickHitCounter >= maxCount)
        {
            _quickHitCounter = 0;
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

[System.Serializable]
public class AttackModifier
{
    public int newDamage;

    public float newThrowForce;
}