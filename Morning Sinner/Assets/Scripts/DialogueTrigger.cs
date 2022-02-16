using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public dialogue[] Dialogue;
    public bool interactedYet;
    public void triggerDialogue()
    {
        if (!interactedYet)
        {
            DialogueManager.instance.startDialogue(Dialogue[0]);
            interactedYet = true;
        }
        else
        {
            DialogueManager.instance.startDialogue(Dialogue[1]);
        }
    }
}
