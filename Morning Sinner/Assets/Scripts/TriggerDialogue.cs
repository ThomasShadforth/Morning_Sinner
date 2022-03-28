using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogue[] dialogueRoutes;
    public float interactionCount;

    public Dialogue selectedDialogue;
    DialogueSO DialogueSource;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && !DialogueLoader.instance.dialogueInProg)
        {
            if(interactionCount == 0)
            {
                DialogueSource = dialogueRoutes[0].dialogue;
            }
            else
            {
                DialogueSource = dialogueRoutes[1].dialogue;
            }

            
            DialogueLoader.instance.SetStartingDialogue(DialogueSource);
            DialogueLoader.instance.StartBranchingDialogue();

            interactionCount++;
        }
    }


}
