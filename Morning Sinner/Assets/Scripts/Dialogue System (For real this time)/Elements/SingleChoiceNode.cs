using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SingleChoiceNode : DialogueNode
{
    

    public override void Initialize(string nodeName, DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(nodeName, graphView, position);

        dialogueType = DialogueType.SingleChoice;

        ChoiceSaveData choiceData = new ChoiceSaveData()
        {
            Text = "Next Dialogue"
        };

        Choices.Add(choiceData);
    }

    public override void Draw()
    {
        base.Draw();

        foreach(ChoiceSaveData choice in Choices)
        {
            Port choicePort = this.CreatePort(choice.Text);

            choicePort.userData = choice;

            outputContainer.Add(choicePort);

            
        }

        RefreshExpandedState();
        RefreshPorts();
    }
}
