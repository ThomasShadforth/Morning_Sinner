using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SingleChoiceNode : DialogueNode
{
    

    public override void Initialize(DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(graphView, position);

        dialogueType = DialogueType.SingleChoice;

        Choices.Add("Next Dialogue");
    }

    public override void Draw()
    {
        base.Draw();

        foreach(string choice in Choices)
        {
            Port choicePort = this.CreatePort(choice);

            choicePort.portName = choice;

            outputContainer.Add(choicePort);

            
        }

        RefreshExpandedState();
        RefreshPorts();
    }
}
