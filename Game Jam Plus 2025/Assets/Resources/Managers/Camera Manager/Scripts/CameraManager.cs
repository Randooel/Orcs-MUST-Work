using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    #region Variables

    #region Script References
    private GameManager _gameManager;
    private GoUI _goUI;
    [PropertySpace(SpaceAfter = 10)]
    public EnemySpawn currentSpawn;
    private QuestManager _questManager;
    #endregion

    #region Camera Move related References
    [TabGroup("Camera Move")]
    BorderCollision _borderCollision;
    [TabGroup("Camera Move")]
    [SerializeField] Transform _camera;
    #endregion

    #region Config
    [TabGroup("Camera Move")]
    [SerializeField] [Range(10, 50)] float _nextPos = 22f;
    [TabGroup("Camera Move")]
    [SerializeField] float _aditionalVertcialDistance = 3f;
    [TabGroup("Camera Move")]
    [SerializeField] string _nextDirection;
    #endregion

    #region Enemy Spawn Config
    [TabGroup("Enemy Spawn Config")]
    public int enemyCounter;
    [TabGroup("Enemy Spawn Config")]
    public bool hasEnemySpawn;
    [TabGroup("Enemy Spawn Config")]
    public EnemyBehavior enemyBehavior;
    #endregion

    #region NPC Related
    [Header("NPC Related")]
    [PropertySpace(SpaceBefore = 15, SpaceAfter = 15)]
    public bool hasNPC;
    public NPCBehavior currentNPC;
    #endregion

    #region Collider Config
    [Header("Collider Config")]
    [SerializeField] BoxCollider2D _cameraViewCollider;
    #endregion

    #endregion

    public string NextDirection { get => _nextDirection; set => _nextDirection = value; }

    void Start()
    {
        _borderCollision = FindAnyObjectByType<BorderCollision>();
        _goUI = FindAnyObjectByType<GoUI>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _questManager = FindAnyObjectByType<QuestManager>();

        CheckForEnemies(false);
        //StartCoroutine(WaitToCheckEnemies(0));
    }

    void Update()
    {
        //Debug Functions
        //TestNavigateTo();
        //TestSetNextDirection();
    }

    #region Move Camera Functions
    public void SetNextDirection(string direction)
    {
        if (direction == "up")
        {
            NextDirection = "up";
            _borderCollision.currentTriggerCollider = 0;

            _borderCollision.colliders[0].isTrigger = true;
        }
        else if (direction == "left")
        {
            NextDirection = "left";
            _borderCollision.currentTriggerCollider = 0;

            _borderCollision.colliders[1].isTrigger = true;
        }
        else if (direction == "right")
        {
            NextDirection = "right";
            _borderCollision.currentTriggerCollider = 0;

            _borderCollision.colliders[2].isTrigger = true;
        }
        else if (direction == "down")
        {
            NextDirection = "down";
            _borderCollision.currentTriggerCollider = 0;

            _borderCollision.colliders[3].isTrigger = true;
        }
    }

    public void NavigateTo(string direction)
    {
        if (direction == "up")
        {
            // Moves player to the next location. Without this code the camera will move faster than the player
            // This will result in the player being trapped in the same room, with no means to advance
            var pMove = FindAnyObjectByType<PlayerMovement>();
            var nxtPos = pMove.gameObject.transform.position;
            pMove.DOMoveSomewhere(new Vector3(nxtPos.x, nxtPos.y + 1, 0), 0);

            // Updates camera position
            _camera.position += new Vector3(0f, (_nextPos / 2 + _aditionalVertcialDistance), 0f);
        }
        else if (direction == "left")
        {
            _camera.position += new Vector3(-_nextPos, 0f, 0f);
        }
        else if (direction == "right")
        {
            _camera.position += new Vector3(_nextPos, 0f, 0f);
        }
        else if (direction == "down")
        {
            var pMove = FindAnyObjectByType<PlayerMovement>();
            var nxtPos = pMove.gameObject.transform.position;
            pMove.DOMoveSomewhere(new Vector3(nxtPos.x, nxtPos.y - 1, 0), 0);

            _camera.position += new Vector3(0f, -(_nextPos/2 + _aditionalVertcialDistance), 0f);
        }

        hasNPC = false;
        _questManager.canUpdateRoom = true;
        enemyCounter = 0;
        _cameraViewCollider.enabled = true;
        _goUI.ToggleUI(false);
        StartCoroutine(WaitToCheckEnemies(0.5f));
    }
    #endregion

    #region Test Scripts
    private void TestNavigateTo()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NavigateTo("left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NavigateTo("right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateTo("up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateTo("down");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _camera.position = Vector3.zero;
        }
    }

    private void TestSetNextDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetNextDirection("left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetNextDirection("right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetNextDirection("up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetNextDirection("down");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetNextDirection("");
        }
    }
    #endregion

    public void CheckForEnemies(bool wasCalledByEnemy)
    {
        //Debug.Log("CheckForEnemies() " + "| WAS CALLED BY ENEMY == " + wasCalledByEnemy + " | hasEnemySpawn == " + hasEnemySpawn);

        if (enemyCounter <= 0)
        {
            if (hasEnemySpawn == true)
            {
                currentSpawn.SpawnEnemy();
            }
            else
            {
                if (wasCalledByEnemy)
                {
                    CleanRoomAnim();
                }
                else
                {
                    if(!hasNPC)
                    {
                        _gameManager.UnlockNextRoom();
                    }
                    else
                    {
                        currentNPC.canTalk = true;
                    }
                }
            }

            if (enemyBehavior != null)
            {
                enemyBehavior = null;
            }
        }
    }

    public void CleanRoomAnim()
    {
        Time.timeScale = 0.35f;
        enemyBehavior.EnableCamera();

        DOVirtual.DelayedCall(1f, () =>
        {
            enemyBehavior = null;

            Time.timeScale = 1;

        }).OnComplete(() =>
        {
            if (!hasNPC)
            {
                _gameManager.UnlockNextRoom();
            }
            else
            {
                currentNPC.canTalk = true;
            }
        });
    }

    #region Coroutine(s)
    private IEnumerator WaitToCheckEnemies(float waitTime)
    {
        // Debug.LogWarning("WaitToCheckEnemies");
        yield return new WaitForSeconds(waitTime);
        //Debug.LogError("WaitedToCheckEnemies");

        CheckForEnemies(false);
    }
    #endregion
}