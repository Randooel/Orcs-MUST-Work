using UnityEngine;
using TMPro;

public class NPCBehavior : MonoBehaviour
{
    private DialogueManager _dialogueManager;

    [Header("Config")]
    [SerializeField] DialogueSO _dialogueSO;
    [SerializeField] int _currentEncounter;
    [SerializeField] int _currentDialogueGroup;

    [Header("Dialogue ¨Visual")]
    [SerializeField] GameObject _dialogueBox;
    [SerializeField] TextMeshPro _dialogueText;

    [Space(10)]
    [SerializeField] PlayerMovement _currentPlayerMovement;

    public int CurrentEncounter { get => _currentEncounter; set => _currentEncounter = value; }

    void Start()
    {
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
            Debug.Log("Collided with " + collision.name);

            _currentPlayerMovement = collision.GetComponentInParent<PlayerMovement>();
            _currentPlayerMovement.canMove = false;

            _dialogueBox.SetActive(true);

            _dialogueManager.SetDialogue(_dialogueSO, _currentDialogueGroup, _dialogueBox, _dialogueText);
            _dialogueManager.PlayDialogue();
        }
    }

    public void OnDialogueEnd()
    {
        _currentPlayerMovement.canMove = true;
        _currentPlayerMovement = null;

        _dialogueBox.SetActive(false);

        //_currentEncounter++;
    }
}
