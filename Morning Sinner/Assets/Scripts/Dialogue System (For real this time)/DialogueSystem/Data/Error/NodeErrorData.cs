using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeErrorData
{
    public DialogueErrorData ErrorData {get; set;}
    public List<DialogueNode> Nodes { get; set; }

    public NodeErrorData()
    {
        ErrorData = new DialogueErrorData();
        Nodes = new List<DialogueNode>();
    }
}
