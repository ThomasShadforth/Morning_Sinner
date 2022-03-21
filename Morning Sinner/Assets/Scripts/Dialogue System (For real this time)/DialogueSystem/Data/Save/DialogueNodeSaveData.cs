using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNodeSaveData
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string NameText { get; set; }
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public List<ChoiceSaveData> Choices { get; set; }
    [field: SerializeField] public string GroupID { get; set; }
    [field: SerializeField] public DialogueType dialogueType { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
}
