using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupSaveData
{
    [SerializeField]
    public string ID { get; set; }
    [SerializeField] public string GroupName { get; set; }
    [SerializeField] public Vector2 Position { get; set; }
    
}
