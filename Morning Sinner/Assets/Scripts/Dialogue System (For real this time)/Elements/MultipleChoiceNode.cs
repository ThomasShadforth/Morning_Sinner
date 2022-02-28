using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

public class MultipleChoiceNode : DialogueNode
{
    

    public override void Initialize(DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(graphView, position);

        dialogueType = DialogueType.MultipleChoice;

        Choices.Add("New Choice");
    }

    public override void Draw()
    {
        base.Draw();

        //MAIN CONTAINER

        Button addChoiceButton = ElementUtilities.CreateButton("Add Choice", () =>
        {
            Port choicePort = CreateChoicePort("New Choice");

            Choices.Add("New Choice");

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        //OUTPUT CONTAINER

        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);

            
        }

        RefreshExpandedState();
        RefreshPorts();
    }

    #region Elements Creation
    private Port CreateChoicePort(string choice)
    {
        Port choicePort = this.CreatePort();

        choicePort.portName = "";

        Button deleteChoiceButton = ElementUtilities.CreateButton("X");

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = ElementUtilities.CreateTextField(choice);

        choiceTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__choice-textfield",
            "ds-node__textfield__hidden"
            );

        

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);
        return choicePort;
    }
    #endregion
}
