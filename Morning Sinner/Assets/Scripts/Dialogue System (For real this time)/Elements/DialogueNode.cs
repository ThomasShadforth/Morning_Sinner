using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

public class DialogueNode : Node
{
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    public DialogueType dialogueType { get; set; }

    public DialogueGroup group { get; set; }

    DialogueGraphView _graphView;

    private Color defaultBackgroundColor;

    public virtual void Initialize(DialogueGraphView graphView,Vector2 position)
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Dialogue Text";

        _graphView = graphView;

        defaultBackgroundColor = new Color(29 / 255f, 29 / 255f, 30 / 255f);

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    #region Overridden Methods
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
        evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
        

        base.BuildContextualMenu(evt);
    }
    #endregion

    public virtual void Draw()
    {
        TextField dialogeNameTextField = ElementUtilities.CreateTextField(DialogueName, null, callback => {

            TextField target = (TextField)callback.target;

            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            if (group == null)
            {
                _graphView.RemoveUngroupedNode(this);

                DialogueName = target.value;

                _graphView.AddUngroupedNode(this);

                return;
            }

            DialogueGroup currentGroup = group;

            _graphView.RemoveGroupedNode(this, group);

            DialogueName = target.value;

            _graphView.AddGroupedNode(this, currentGroup);
        });

        dialogeNameTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__filename-textfield",
            "ds-node__textfield__hidden"
            );

        

        titleContainer.Insert(0, dialogeNameTextField);

        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

        inputPort.portName = "Dialogue Connection";

        inputContainer.Add(inputPort);

        VisualElement customDataContainer = new VisualElement();

        customDataContainer.AddToClassList("ds-node__custom-data-container");

        Foldout textFoldout = ElementUtilities.CreateFoldout("Dialogue Text");

        TextField textTextField = ElementUtilities.CreateTextArea(Text);

        textTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__quote-textfield"
            );

        

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);

        
    }
    #region Utility Methods
    public void DisconnectAllPorts() {
        DisconnectInputPorts();
        DisconnectOutputPorts();
    }

    void DisconnectInputPorts()
    {
        DisconnectPorts(inputContainer);
    }

    void DisconnectOutputPorts()
    {
        DisconnectPorts(outputContainer);
    }

    void DisconnectPorts(VisualElement container)
    {
        foreach(Port port in container.Children())
        {
            if (!port.connected)
            {
                continue;
            }

            _graphView.DeleteElements(port.connections);
        }
    }

    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }

    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = defaultBackgroundColor;
    }
    #endregion
}
