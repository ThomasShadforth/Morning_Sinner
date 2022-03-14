using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoiceData
{
    [SerializeField] public string Text { get; set; }
    [SerializeField] public DialogueSO NextDialogue { get; set; }
}
