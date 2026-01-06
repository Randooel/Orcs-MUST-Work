using TMPro;
using UnityEngine;
using EasyTextEffects;

public class DialogueManager : MonoBehaviour
{
    private DialogueUI _dialogueUI;
    private NPCBehavior _npcBehavior;

    public DialogueSO _currentDialogueSO;

    [SerializeField] GameObject _currentDialogueBox;
    [SerializeField] TextMeshPro _currentDialogueText;

    [Header("Dialogue Group indexes")]
    [SerializeField] private int _currentLine = 0;
    //[SerializeField] private int _currentEncounter = 0;
    [SerializeField] private int _currentDialogueGroup = 0;
    [SerializeField][Range(0, 10)] private int maxDialogueGroup;
    [SerializeField] private int currentEncounter;
    [SerializeField] public bool isOnDialogue;

    [Space(10)]
    [SerializeField] TextEffect _textEffect;

    public int CurrentLine { get => _currentLine; set => _currentLine = value; }
    public int CurrentDialogueGroup { get => _currentDialogueGroup; set => _currentDialogueGroup = value; }
    public int MaxDialogueGroup { get => maxDialogueGroup; set => maxDialogueGroup = value; }
    public int CurrentEncounter { get => currentEncounter; set => currentEncounter = value; }


    void Start()
    {
        _dialogueUI = GetComponent<DialogueUI>();
    }

    void Update()
    {
        if(isOnDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckDialogue();
            }
        }
    }

    public void SetDialogue(NPCBehavior nBehavior, DialogueSO dialogue, int dialogueGroup, GameObject dBox, TextMeshPro text)
    {
        _npcBehavior = nBehavior;
        _textEffect = _npcBehavior.gameObject.GetComponentInChildren<TextEffect>();

        _currentDialogueSO = dialogue;
        CurrentDialogueGroup = dialogueGroup;
        
        _currentDialogueText = text;
        _currentDialogueBox = dBox;

        _dialogueUI.DOIntroBars();
        PlayDialogue();
    }

    public void PlayDialogue()
    {
        isOnDialogue = true;

        var dialogue = _currentDialogueSO.encounters[CurrentEncounter].dialogueGroups[CurrentDialogueGroup];

        _currentDialogueText.text = dialogue.npcLines[CurrentLine];

        _textEffect.StopAllEffects();
        _textEffect.Refresh();

        _textEffect.StartManualEffect("Typewritter");

        _npcBehavior.animator.SetTrigger("Speak");
        //_npcBehavior.animator.Play("Speak");
    }

    public void CheckDialogue()
    {
        var dialogue = _currentDialogueSO.encounters[CurrentEncounter].dialogueGroups[CurrentDialogueGroup];

        if (CurrentLine >= dialogue.npcLines.Count -1)
        {
            EndDialogue();
        }
        else
        {
            CurrentLine++;

            PlayDialogue();
        }
    }

    private void ShowPlayerResponses()
    {
        Debug.Log("Show Player responses");
    }

    public void EndDialogue()
    {
        isOnDialogue = false;

        _dialogueUI.DOExitBars();

        _npcBehavior.OnDialogueEnd();

        _currentLine = 0;

        _currentDialogueText = null;
        _currentDialogueBox = null;

        _currentDialogueSO = null;
        CurrentDialogueGroup = 0;
    }
}
