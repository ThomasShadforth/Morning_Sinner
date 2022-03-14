using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainerSO : ScriptableObject
{
    [SerializeField] public string FileName { get; set; }
    [SerializeField] public SerializableDictionary<DialogueGroupSO, List<DialogueSO>> DialogueGroups { get; set; }
    [SerializeField] public List<DialogueSO> UngroupedDialogues { get; set; }

    public void Initialize(string fileName)
    {
        FileName = fileName;

        DialogueGroups = new SerializableDictionary<DialogueGroupSO, List<DialogueSO>>();
        UngroupedDialogues = new List<DialogueSO>();
    }
}
