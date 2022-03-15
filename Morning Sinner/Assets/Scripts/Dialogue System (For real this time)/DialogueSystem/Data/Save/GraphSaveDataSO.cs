using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSaveDataSO : ScriptableObject
{
    [field: SerializeField] public string FileName { get; set; }
    [field: SerializeField] public List<GroupSaveData> Groups { get; set; }
    [field: SerializeField] public List<DialogueNodeSaveData> Nodes { get; set; }
    
    [field: SerializeField] public List<string> OldGroupNames { get; set; }
    [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
    [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

    public void Initialize(string fileName)
    {
        FileName = fileName;

        Groups = new List<GroupSaveData>();
        Nodes = new List<DialogueNodeSaveData>();
    }
}
