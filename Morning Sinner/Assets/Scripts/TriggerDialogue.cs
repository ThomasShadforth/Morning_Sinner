using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogue selectedDialogue;
    DialogueSO DialogueSource;

    // Start is called before the first frame update
    void Start()
    {
        DialogueSource = selectedDialogue.dialogue;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && !DialogueLoader.instance.dialogueInProg)
        {
            Debug.Log("STARTING");
            DialogueLoader.instance.SetStartingDialogue(DialogueSource);
            DialogueLoader.instance.StartBranchingDialogue();
        }
    }


}
