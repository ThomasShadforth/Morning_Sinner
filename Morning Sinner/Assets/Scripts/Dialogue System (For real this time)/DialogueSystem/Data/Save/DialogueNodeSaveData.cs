using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNodeSaveData
{
    [SerializeField] public string ID { get; set; }
    [SerializeField] public string Name { get; set; }
    [SerializeField] public string Text { get; set; }
    [SerializeField] public List<ChoiceSaveData> Choices { get; set; }
    [SerializeField] public string GroupID { get; set; }
    [SerializeField] public DialogueType dialogueType { get; set; }
    [SerializeField] public Vector2 Position { get; set; }
}
