using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("References")]
    BorderCollision _borderCollision;
    [SerializeField] Transform _camera;

    [Header("Config")]
    [SerializeField] [Range(10, 50)] float _nextPos = 22f;
    [SerializeField] string _nextDirection;

    [Header("Collider Config")]
    [SerializeField] BoxCollider2D _cameraViewCollider;
    [SerializeField] string targetTag = "Enemy";

    public int enemyCounter;

    public string NextDirection { get => _nextDirection; set => _nextDirection = value; }

    void Start()
    {
        _borderCollision = FindAnyObjectByType<BorderCollision>();

        _gameManager = FindAnyObjectByType<GameManager>();

        SetEnemyCounter();
    }

    void Update()
    {
        //Debug Functions
        //TestNavigateTo();
        TestSetNextDirection();
    }

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
            _camera.position += new Vector3(0f, _nextPos/2, 0f);
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
            _camera.position += new Vector3(0f, -_nextPos/2, 0f);
        }

        SetEnemyCounter();
    }

    // TEST SCRIPTS
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

    public void SetEnemyCounter()
    {
        _cameraViewCollider.enabled = true;
        StartCoroutine(WaitToCheckEnemies());
    }

    public void CheckForEnemies()
    {
        if(enemyCounter == 0)
        {
            _gameManager.UnlockNextRoom();
        }
    }

    // COROUTINES
    private IEnumerator WaitToCheckEnemies()
    {
        yield return new WaitForSeconds(0.5f);
        CheckForEnemies();
    }
}
