using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private NPCBehavior _npcBehavior;

    public DialogueSO _currentDialogueSO;

    [SerializeField] GameObject _currentDialogueBox;
    [SerializeField] TextMeshPro _currentDialogueText;

    [Header("Dialogue Group indexes")]
    [SerializeField] private int _currentLine = 0;
    [SerializeField] private int _currentEncounter = 0;
    [SerializeField] private int _currentDialogueGroup = 0;
    [SerializeField][Range(0, 10)] private int maxDialogueGroup;
    [SerializeField] private int currentEncounter;

    public int CurrentLine { get => _currentLine; set => _currentLine = value; }
    public int CurrentDialogueGroup { get => _currentDialogueGroup; set => _currentDialogueGroup = value; }
    public int MaxDialogueGroup { get => maxDialogueGroup; set => maxDialogueGroup = value; }
    public int CurrentEncounter { get => currentEncounter; set => currentEncounter = value; }


    void Start()
    {
        
    }

    void Update()
    {


        // DEBUG SCRIPT
        /*
        if(Input.GetMouseButtonDown(0))
        {
            PlayDialogue();
        }
        */
    }

    public void SetDialogue(DialogueSO dialogue, int dialogueGroup, GameObject dBox, TextMeshPro text)
    {
        _currentDialogueSO = dialogue;
        CurrentDialogueGroup = dialogueGroup;
        
        _currentDialogueText = text;
        _currentDialogueBox = dBox;
    }

    public void PlayDialogue()
    {
        var dialogue = _currentDialogueSO.encounters[CurrentEncounter].dialogueGroups[CurrentDialogueGroup];
        _currentDialogueText.text = dialogue.npcLines[CurrentLine];

        CurrentLine++;

        if(CurrentLine > dialogue.npcLines.Count && dialogue.playerResponses != null)
        {
            EndDialogue();
            ShowPlayerResponses();
        }
    }

    private void ShowPlayerResponses()
    {
        Debug.Log("Show Player responses");
    }

    public void EndDialogue()
    {
        _currentDialogueText = null;
        _currentDialogueBox = null;

        _currentDialogueSO = null;
        CurrentDialogueGroup = 0;

        _npcBehavior.OnDialogueEnd();
    }
}
