using UnityEngine;
using DG.Tweening;
using System.Collections;

public abstract class EnemyBehavior : MonoBehaviour
{
    [Header("References")]
    protected Animator _animator;
    [SerializeField] protected Transform _parent;

    [Header("Config")]
    [SerializeField] protected State _currentState;
    protected enum State
    {
        Idle,
        Chase,
        Attack1,
        Attack2,
        Attack3,
        TakeDamage,
        Death
    }
    [SerializeField] protected bool _isChasing;

    [Space(10)]
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _maxHealth;

    [Space(5)]
    [SerializeField] protected int _damage1;
    [SerializeField] private int damage2;
    [SerializeField] private int damage3;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _attackRate;

    [Header("Target Config")]
    [SerializeField] Transform _currentTarget;
    [Space(5)]
    [SerializeField] protected Transform[] _playerTargets; // [0]: up / [1]: left / [2]: right / [3]: down

    [Header("Colliders Config")]
    [SerializeField] protected EnemyCollisionDetection _attackRange;
    [SerializeField] protected EnemyCollisionDetection _detectionRange;
    [SerializeField] protected EnemyCollisionDetection _attackCollider;
    [SerializeField] protected Rigidbody2D m_rigidbody;
    [SerializeField] protected Vector2 _currentTrajectory;

    public int Damage1 { get => _damage1; set => _damage1 = value; }
    protected int Damage2 { get => damage2; set => damage2 = value; }
    protected int Damage3 { get => damage3; set => damage3 = value; }

    protected virtual void Start()
    {
        // Seting variables up
        RestoreHealth(_maxHealth);
        SwitchState(State.Idle);

        if(_detectionRange == null)
        {
            Debug.Log("Detection Range collider was not found!");
        }
        if (_attackRange == null)
        {
            Debug.Log("Attack Range collider was not found!");
        }
        if(m_rigidbody == null)
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        // Seting references up
        _animator = GetComponent<Animator>();
        _parent = GetComponentInParent<Transform>();

        _playerTargets = new Transform[4];

        for (int i = 0; i < _playerTargets.Length; i++)
        {
            _playerTargets[i] = GameObject.Find("Target" + i).transform;
        }
    }

    protected virtual void Update()
    {
        if (_isChasing)
        {
            Vector3 direction = (_currentTarget.position - _parent.position).normalized;

            _parent.position += direction * _speed * Time.deltaTime;

            if(_currentTarget.position.x < _parent.position.x)
            {
                _parent.rotation = Quaternion.Euler(0, 180f, 0);
            }
            if(_currentTarget.position.x > _parent.position.x)
            {
                _parent.rotation = Quaternion.Euler(0, 360f, 0);
            }
        }

        // DEBUG SCRIPTS
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(_currentState != State.Chase)
            {
                SwitchState(State.Chase);
            }
            else
            {
                SwitchState(State.Idle);
            }
        }
    }

    // COLLISION AND HEALTH RELETADE FUNCTIONS
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arm"))
        {
            if (collision.transform.parent.TryGetComponent(out PlayerAttacks1 playerAttacks1))
            {
                int dmg = collision.GetComponentInParent<PlayerAttacks1>().CurrentDamage;
                float throwForce = collision.GetComponentInParent<PlayerAttacks1>().CurrentThrowForce;
                var direction = collision.transform;

                HandleTakeDamage(dmg);

                HandleThrown(throwForce, direction);
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log(collision.gameObject.name);
    
            var direction = _currentTrajectory;

            float force = m_rigidbody.linearVelocity.magnitude * 2f;

            //HandleThrown(force, direction);
        }
    }

    protected void RestoreHealth(int heal)
    {
        if (heal + _currentHealth >= _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        else
        {
            _currentHealth += heal;
        }
    }

    // STATE MACHINE RELATED FUNCTIONS
    protected void SwitchState(State nextState)
    {
        _isChasing = false;

        _currentState = nextState;

        switch(nextState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chase:
                HandleChase(); 
                break;
            case State.Attack1:
                HandleAttack1();
                break;
            case State.Attack2:
                HandleAttack2(); 
                break;
            case State.Attack3:
                HandleAttack3(); 
                break;
            // HandleTakeDamage() is handled by the collision event
            case State.Death:
                HandleDeath();
                break;
        }
    }

    protected virtual void HandleIdle()
    {
        
    }
    protected virtual void HandleChase()
    {
        var rand = Random.Range(0, _playerTargets.Length);

        _currentTarget = _playerTargets[rand];
        _attackRange.gameObject.SetActive(true);

        _isChasing = true;
        _animator.SetTrigger("chase");

        _currentTrajectory = Vector3.zero;
    }
    protected virtual void HandleAttack1()
    {
        _animator.SetTrigger("attack1");
    }
    protected virtual void HandleAttack2()
    {
        _animator.SetTrigger("attack2");
    }
    protected virtual void HandleAttack3()
    {
        _animator.SetTrigger("attack3");
    }
    protected virtual void HandleTakeDamage(int damage)
    {
        _animator.SetTrigger("takeDamage");

        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            SwitchState(State.Death);
        }
    }

    protected virtual void HandleThrown(float tForce, Transform collisionDirection)
    {
        var trajectory = (transform.position - collisionDirection.position).normalized;
        _currentTrajectory = trajectory;

        m_rigidbody.AddForce(trajectory * tForce, ForceMode2D.Impulse);
    }

    protected virtual void HandleDeath()
    {
        _animator.SetTrigger("death");

        var collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }

    public void ToggleState()
    {
        if (_detectionRange.collisionDetected)
        {
            _detectionRange.gameObject.SetActive(false);

            SwitchState(State.Chase);
        }
        if (_attackRange.collisionDetected)
        {
            _attackRange.gameObject.SetActive(false);

            SwitchState(State.Attack1);
        }
    }

    public void HideObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator WaitToAttackAgain()
    {
        yield return new WaitForSeconds(_attackRate);

        _attackRange.gameObject.SetActive(true);
    }
}
