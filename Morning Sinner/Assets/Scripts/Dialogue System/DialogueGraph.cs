using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();

        window.titleContent = new GUIContent(text: "Dialogue Graph");
    }


    void ConstructGraph()
    {
        _graphView = new DialogueGraphView
        {
            name = "DialogueGraph"
        };

        _graphView.StretchToParentSize();

        rootVisualElement.Add(_graphView);
    }

    private void OnEnable()
    {
        ConstructGraph();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(clickEvent: () =>
        {
            _graphView.CreateNode("Dialogue Node");
        });

        nodeCreateButton.text = "Create Node";

        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }
}
