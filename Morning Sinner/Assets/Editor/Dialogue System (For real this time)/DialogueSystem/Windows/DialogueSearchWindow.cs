using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
{
    DialogueGraphView _graphView;
    Texture2D indentationIcon;
    public void Initialize(DialogueGraphView graphView)
    {
        _graphView = graphView;
        indentationIcon = new Texture2D(1, 1);
        indentationIcon.SetPixel(0, 0, Color.clear);
        indentationIcon.Apply();
    }
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        

        //Search tree essentially creates the search menu
        //Search tree positions begin from index 0, and essentially determine what
        //search elements appear at what position
        List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
        {
            //Position 0 indicates a title
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            //Position 1 indicates a menu that can be selected, whereas 2 onward indicate
            //Elements that can be found in sub menus
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon))
            {
                level = 2,
                userData = DialogueType.SingleChoice
            },
            new SearchTreeEntry(new GUIContent("Multiple Choice", indentationIcon))
            {
                level = 2,
                userData = DialogueType.MultipleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
            new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
            {
                level = 2,
                userData = new Group()
            }
        };

        return searchTreeEntries;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 localMousePosition = _graphView.getLocalMousePosition(context.screenMousePosition, true);
        switch (SearchTreeEntry.userData)
        {
            case DialogueType.SingleChoice:
                SingleChoiceNode singleChoiceNode = (SingleChoiceNode) _graphView.CreateNode("DialogueName", DialogueType.SingleChoice, localMousePosition);

                _graphView.AddElement(singleChoiceNode);
                return true;
                

            case DialogueType.MultipleChoice:
                MultipleChoiceNode multipleChoiceNode = (MultipleChoiceNode)_graphView.CreateNode("DialogueName", DialogueType.MultipleChoice, localMousePosition);

                _graphView.AddElement(multipleChoiceNode);
                return true;
                

            case Group _:
                _graphView.CreateGroup("DialogueGroup", localMousePosition);

                
                return true;

            default:
                return false;
        }

        
    }
}
