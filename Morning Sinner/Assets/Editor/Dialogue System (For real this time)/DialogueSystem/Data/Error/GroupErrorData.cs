using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Experimental;
using UnityEngine;

public class GroupErrorData
{
    public DialogueErrorData ErrorData { get; set; }
    public List<DialogueGroup> Groups { get; set; }

    public GroupErrorData()
    {
        ErrorData = new DialogueErrorData();

        Groups = new List<DialogueGroup>();
    }
}
