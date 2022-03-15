using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    

    //Scriptable objects
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueContainerSO dialogueContainer;
    [SerializeField] private DialogueGroupSO dialogueGroup;

    //Filters
    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialoguesOnly;
}
