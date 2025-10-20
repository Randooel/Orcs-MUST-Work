using UnityEngine;
using TMPro;
using EasyTextEffects;

public class NPCBehavior : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    public Animator animator;

    [Header("Config")]
    [SerializeField] DialogueSO _dialogueSO;
    [SerializeField] int _currentEncounter;
    [SerializeField] int _currentDialogueGroup;

    [Header("Dialogue ¨Visual")]
    [SerializeField] GameObject _dialogueBox;
    [SerializeField] TextMeshPro _dialogueText;
    [SerializeField] GameObject _npcCamera;
    [SerializeField] Transform _playerPosition;

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
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arm"))
        {
            _currentPlayerMovement = collision.GetComponentInParent<PlayerMovement>();
            _currentPlayerMovement.canMove = false;
            _currentPlayerMovement.StopWalk();

            _dialogueBox.SetActive(true);
            EnableCamera();

            _currentPlayerMovement.DOMoveSomewhere(transform.position, _playerPosition.position, 0.5f);

            _dialogueManager.SetDialogue(this, _dialogueSO, _currentDialogueGroup, _dialogueBox, _dialogueText);
        }
    }

    public void OnDialogueEnd()
    {
        _currentPlayerMovement.canMove = true;
        _currentPlayerMovement = null;

        DisableCamera();

        _dialogueBox.SetActive(false);

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
}
