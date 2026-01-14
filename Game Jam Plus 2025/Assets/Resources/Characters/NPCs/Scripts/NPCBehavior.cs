using UnityEngine;
using TMPro;
using EasyTextEffects;
using System.ComponentModel;

public class NPCBehavior : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    public Animator animator;

    #region Config
    [Header("Config")]
    [SerializeField] DialogueSO _dialogueSO;
    [SerializeField] int _currentEncounter;
    [SerializeField] int _currentDialogueGroup;
    #endregion

    #region Dialogue Visual
    [Header("Dialogue ¨Visual")]
    [SerializeField] GameObject _dialogueBox;
    [SerializeField] TextMeshPro _dialogueText;
    [SerializeField] GameObject _npcCamera;
    [SerializeField] Transform _playerPosition;
    #endregion

    [Space(10)]
    [Sirenix.OdinInspector.ReadOnly]
    public bool canTalk;

    [Space(10)]
    [SerializeField] PlayerMovement _currentPlayerMovement;

    public int CurrentEncounter { get => _currentEncounter; set => _currentEncounter = value; }

    void Start()
    {
        // Setting this script up
        DisableCamera();

        // Setting references up
        animator = GetComponent<Animator>();
        _dialogueManager = FindAnyObjectByType<DialogueManager>();

        _dialogueBox.SetActive(false);

        // Changing the dialogue's text Order in Layer
        var renderer = _dialogueText.gameObject.GetComponent<Renderer>();
        renderer.sortingOrder = 16;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arm") && canTalk)
        {
            _currentPlayerMovement = collision.GetComponentInParent<PlayerMovement>();
            _currentPlayerMovement.canMove = false;
            _currentPlayerMovement.StopWalk();

            _dialogueBox.SetActive(true);
            EnableCamera();

            _currentPlayerMovement.DOMoveToNPC(transform.position, _playerPosition.position, 0.5f);

            _dialogueManager.SetDialogue(this, _dialogueSO, _currentDialogueGroup, _dialogueBox, _dialogueText);
        }
    }

    public void PlayIdle()
    {
        //animator.Play("Idle");
        animator.SetTrigger("Idle");
    }

    public void OnDialogueEnd()
    {
        _currentPlayerMovement.canMove = true;
        _currentPlayerMovement = null;

        DisableCamera();

        ResetAllTriggers();
        //animator.Play("Idle");
        animator.SetTrigger("Idle");

        _dialogueBox.SetActive(false);

        FindAnyObjectByType<GameManager>().UnlockNextRoom();

        //_currentEncounter++;
    }

    public void EnableCamera()
    {
        _npcCamera.SetActive(true);
    }

    public void DisableCamera()
    {
        _npcCamera.SetActive(false);
    }

    // ANIMATION
    private void ResetAllTriggers()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
}
