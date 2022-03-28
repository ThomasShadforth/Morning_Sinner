using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupSaveData
{
    [field: SerializeField]
    public string ID { get; set; }
    [field: SerializeField] public string GroupName { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
    
}
