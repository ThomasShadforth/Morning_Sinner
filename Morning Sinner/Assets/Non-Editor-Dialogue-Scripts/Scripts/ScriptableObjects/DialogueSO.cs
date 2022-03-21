using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSO : ScriptableObject
{
    [field: SerializeField] public string DialogueName { get; set; }
    [field: SerializeField] [field: TextArea()] public string DialogueText { get; set; }
    [field: SerializeField] [field: TextArea()] public string NameText { get; set; }
    [field: SerializeField] public List<DialogueChoiceData> Choices { get; set; }
    [field: SerializeField] public DialogueType dialogueType { get; set; }
    [field: SerializeField] public bool IsStartingDialogue { get; set; }

    public void Initialize(string dialogueName, string nameText, string dialogueText, List<DialogueChoiceData> choices, DialogueType type, bool isStartingDialogue)
    {
        DialogueName = dialogueName;
        DialogueText = dialogueText;
        NameText = nameText;
        Choices = choices;
        dialogueType = type;
        IsStartingDialogue = isStartingDialogue;
    }
}
