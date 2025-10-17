using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTextEffects.Editor.MyBoxCopy.Attributes;

[System.Serializable]
public class DialogueGroup
{
    [Header("Dialogue Info")]
    public string dialogueTag;

    [Header("Dialogue")]
    [TextArea(1, 3)]
    public List<string> npcLines;

    public List<PlayerResponsesList> playerResponses = new List<PlayerResponsesList>();
}

// [x]
[System.Serializable]
public class PlayerResponsesList
{
    [TextArea(1, 3)]
    public string responseLine;

    [Space(10)]
    [Range(-6, 5)] public int answerSatisfaction;

    [Space(10)]
    public bool skipToDialogueX;

    [ConditionalField(nameof(skipToDialogueX), false)]
    public string nextDialogueTag;
}