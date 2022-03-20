using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoiceData
{
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public DialogueSO NextDialogue { get; set; }
}
