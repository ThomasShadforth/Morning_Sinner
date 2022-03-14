using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSO : ScriptableObject
{
    [SerializeField] public string DialogueName { get; set; }
    [SerializeField] [field: TextArea()] public string DialogueText { get; set; }
    [SerializeField] public List<DialogueChoiceData> Choices { get; set; }
    [SerializeField] public DialogueType dialogueType { get; set; }
    [SerializeField] public bool IsStartingDialogue { get; set; }

    public void Initialize(string dialogueName, string dialogueText, List<DialogueChoiceData> choices, DialogueType type, bool isStartingDialogue)
    {
        DialogueName = dialogueName;
        DialogueText = dialogueText;
        Choices = choices;
        dialogueType = type;
        IsStartingDialogue = isStartingDialogue;
    }
}
