using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

public class MultipleChoiceNode : DialogueNode
{
    

    public override void Initialize(string nodeName, DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(nodeName, graphView, position);

        dialogueType = DialogueType.MultipleChoice;

        ChoiceSaveData choiceData = new ChoiceSaveData()
        {
            Text = "New Choice"
        };

        Choices.Add(choiceData);
    }

    public override void Draw()
    {
        base.Draw();

        //MAIN CONTAINER

        Button addChoiceButton = ElementUtilities.CreateButton("Add Choice", () =>
        {
            ChoiceSaveData choiceData = new ChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);

            Port choicePort = CreateChoicePort(choiceData);

            

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        //OUTPUT CONTAINER

        foreach (ChoiceSaveData choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);

            
        }

        RefreshExpandedState();
        RefreshPorts();
    }

    #region Elements Creation
    private Port CreateChoicePort(object userData)
    {
        Port choicePort = this.CreatePort();

        choicePort.userData = userData;

        ChoiceSaveData choiceData = (ChoiceSaveData)userData;

        choicePort.portName = "";

        Button deleteChoiceButton = ElementUtilities.CreateButton("X", () => { 
            if(Choices.Count == 1)
            {
                return;
            }

            if (choicePort.connected)
            {
                _graphView.DeleteElements(choicePort.connections);
            }

            Choices.Remove(choiceData);

            _graphView.RemoveElement(choicePort);
        });

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = ElementUtilities.CreateTextField(choiceData.Text, null, callback => {
            choiceData.Text = callback.newValue;
        });

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
