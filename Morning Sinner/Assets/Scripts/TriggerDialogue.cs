using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogue[] dialogueRoutes;
    public float interactionCount;

    [SerializeField] bool multipleInteractions;

    public Dialogue selectedDialogue;
    [SerializeField] DialogueSO DialogueSource;

    [SerializeField] PlayableDirector cutsceneTimeline;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && !DialogueLoader.instance.dialogueInProg)
        {
            DialogueTrig();
        }
    }

    public void DialogueTrig()
    {
        if (!DialogueLoader.instance.dialogueInProg)
        {
            if (multipleInteractions)
            {
                if (interactionCount == 0)
                {
                    DialogueSource = dialogueRoutes[0].dialogue;
                }
                else
                {
                    DialogueSource = dialogueRoutes[1].dialogue;
                }
            }
            else
            {
                DialogueSource = dialogueRoutes[0].dialogue;
            }



            DialogueLoader.instance.SetStartingDialogue(DialogueSource);
            DialogueLoader.instance.StartBranchingDialogue(cutsceneTimeline);

            interactionCount++;
        }
    }
}
