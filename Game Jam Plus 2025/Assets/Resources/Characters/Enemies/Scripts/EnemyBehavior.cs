using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.VFX;

public abstract class EnemyBehavior : MonoBehaviour
{
    [Header("References")]
    protected Animator _animator;
    [SerializeField] protected Transform _visual;
    [SerializeField] GameObject _enemycamera;

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
    Vector2 lastVelocity;
    float bounceFactor = 0.8f;

    private bool _isDuringThrow;

    [Space(10)]
    #region VFX
    [SerializeField] protected GameObject hit;
    [SerializeField] protected GameObject explosion;
    #endregion


    public int Damage1 { get => _damage1; set => _damage1 = value; }
    protected int Damage2 { get => damage2; set => damage2 = value; }
    protected int Damage3 { get => damage3; set => damage3 = value; }

    protected virtual void Start()
    {
        // Seting variables up
        RestoreHealth(_maxHealth);
        SwitchState(State.Idle);

        hit.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);

        DisableCamera();

        if (_detectionRange == null)
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
            Vector3 direction = (_currentTarget.position - transform.position).normalized;

            transform.position += direction * _speed * Time.deltaTime;

            if(_currentTarget.position.x < _visual.position.x)
            {
                _visual.rotation = Quaternion.Euler(0, 180f, 0);
            }
            if(_currentTarget.position.x > _visual.position.x)
            {
                _visual.rotation = Quaternion.Euler(0, 360f, 0);
            }
        }

        // DEBUG SCRIPTS
        /*
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
        */
    }

    protected void FixedUpdate()
    {
        lastVelocity = m_rigidbody.linearVelocity;

        if(_isDuringThrow)
        {
            if(m_rigidbody.linearVelocity.magnitude > 0)
            {
                _isDuringThrow = false;
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
                _isDuringThrow = true;

                int dmg = collision.GetComponentInParent<PlayerAttacks1>().CurrentDamage;
                float throwForce = collision.GetComponentInParent<PlayerAttacks1>().CurrentThrowForce;
                var direction = collision.transform;

                _isDuringThrow = true;

                HandleTakeDamage(dmg);

                HandleThrown(throwForce, direction);

                hit.gameObject.SetActive(true);
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    hit.gameObject.SetActive(false);
                });
                //hit.gameObject.GetComponentInChildren<VisualEffect>().Play();

                // Combo Increase
                ComboManager.OnHit?.Invoke(true);

                collision.GetComponentInParent<PlayerRage>().RefreshRage(dmg /2);
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 normal = collision.contacts[0].normal;
            m_rigidbody.linearVelocity = Vector2.Reflect(lastVelocity, normal) * bounceFactor;

            if (collision.gameObject.CompareTag("Enemy"))
            {
                var direction = collision.transform;
                var enemy = collision.gameObject.GetComponent<EnemyBehavior>();

                enemy.HandleThrown(50f, direction);

                _currentHealth -= 1;
                enemy._currentHealth -= 1;
                enemy.SwitchState(State.Chase);
            }
        }
        /*
        if(collision.gameObject.CompareTag("Player"))
        {
            var direction = collision.transform;

            HandleThrown(15f, direction);

            var player = collision.gameObject.GetComponent<PlayerHealth>();

            //collision.gameObject.GetComponent<PlayerHealth>().PlayVFX();
            if(collision.gameObject.GetComponent<PlayerRage>().isOnRage == true && _isDuringThrow)
            {
                return;
            }
            else
            {
                Debug.Log(_isDuringThrow);
                player.TakeDamage(1);
            }
        }
        */
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
        _isDuringThrow = false;

        _animator.SetTrigger("chase");
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

        m_rigidbody.AddForce(trajectory * tForce, ForceMode2D.Impulse);
    }

    protected virtual void HandleDeath()
    {
        _animator.SetTrigger("death");

        /*
        var collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        */

        explosion.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.1f, () =>
        {
            explosion.gameObject.SetActive(false);
        });

        // Checks if there is any enemy leftt on the current room
        var cameraManager = FindAnyObjectByType<CameraManager>();
        cameraManager.enemyCounter--;
        cameraManager.enemyBehavior = this;
        cameraManager.CheckForEnemies(true);

        //StartCoroutine(WaitToResetVFX());
        //explosion.gameObject.GetComponentInChildren<VisualEffect>().Play();
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

    public void EnableCamera()
    {
        _enemycamera.SetActive(true);
    }

    public void DisableCamera()
    {
        _enemycamera.SetActive(false);
    }

    // COROUTINES
    IEnumerator WaitToAttackAgain()
    {
        yield return new WaitForSeconds(_attackRate);

        _attackRange.gameObject.SetActive(true);
    }

    IEnumerator WaitToResetVFX()
    {
        yield return new WaitForSeconds(0.5f);

        hit.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);
    }
}
