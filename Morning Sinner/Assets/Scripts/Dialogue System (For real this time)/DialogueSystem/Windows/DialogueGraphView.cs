using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    DialogueSearchWindow searchWindow;
    DialogueEditor editorWindow;

    private MiniMap miniMap;

    SerializableDictionary<string, NodeErrorData> ungroupedNodes;
    SerializableDictionary<string, GroupErrorData> groups;
    SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>> groupedNodes;
    private int repeatedNamesAmount;

    public int RepeatedNamesAmount
    {
        get
        {
            return repeatedNamesAmount;
        }
        set
        {
            repeatedNamesAmount = value;

            if(repeatedNamesAmount == 0)
            {
                editorWindow.EnableSaving();
            }

            if(repeatedNamesAmount == 1)
            {
                editorWindow.DisableSaving();
            }
        }
    }

    public DialogueGraphView(DialogueEditor dEditorWindow)
    {
        editorWindow = dEditorWindow;

        ungroupedNodes = new SerializableDictionary<string, NodeErrorData>();
        groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>>();
        groups = new SerializableDictionary<string, GroupErrorData>();

        AddManipulators();
        AddSearchWindow();
        AddMiniMap();
        AddGridBackground();
        
        OnElementDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGroupRenamed();
        OnGraphViewChanged();

        AddStyles();
        AddMiniMapStyles();
    }

    #region Overridden Methods
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port => { 
            if(startPort == port)
            {
                return;
            }

            if(startPort.node == port.node)
            {
                return;
            }

            if(startPort.direction == port.direction)
            {
                return;
            }

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
    #endregion

    #region Elements Addition

    void AddSearchWindow()
    {
        if (searchWindow == null)
        {
            searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();

            searchWindow.Initialize(this);
        }

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    void AddMiniMap()
    {
        miniMap = new MiniMap()
        {
            //If anchored, can't move the minimap around
            anchored = true
        };

        miniMap.SetPosition(new Rect(15, 50, 200, 180));

        Add(miniMap);
    }

    void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }

    void AddStyles()
    {
        this.AddStyleSheets("DialogueGraph.uss", "NodeStyles.uss");
        
    }

    void AddMiniMapStyles()
    {
        StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
        StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));
        miniMap.style.backgroundColor = backgroundColor;
        miniMap.style.borderTopColor = borderColor;
        miniMap.style.borderBottomColor = borderColor;
        miniMap.style.borderLeftColor = borderColor;
        miniMap.style.borderRightColor = borderColor;
    }

    #endregion

    #region Manipulators
    void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new RectangleSelector());
        

        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultipleChoice));
        this.AddManipulator(CreateGroupContextualMenu());
    }

    IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName", dialogueType, getLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        return contextualMenuManipulator;
    }

    IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Dialogue Group", actionEvent => CreateGroup("Dialogue Group", getLocalMousePosition(actionEvent.eventInfo.localMousePosition))
            ));

        return contextualMenuManipulator;
    }
    #endregion

    #region Elements Creation
    public DialogueGroup CreateGroup(string title, Vector2 localMousePosition)
    {
        DialogueGroup group = new DialogueGroup(title, localMousePosition);

        AddGroup(group);

        AddElement(group);

        foreach(GraphElement selectedElement in selection)
        {
            if(!(selectedElement is DialogueNode))
            {
                continue;
            }

            DialogueNode node = (DialogueNode)selectedElement;
            group.AddElement(node);
        }

        return group;
    }

    

    public DialogueNode CreateNode(string nodeName, DialogueType dialogueType, Vector2 position, bool shouldDraw = true)
    {
        Type nodeType = Type.GetType($"{dialogueType}Node");

        DialogueNode node = (DialogueNode) Activator.CreateInstance(nodeType);


        node.Initialize(nodeName, this ,position);

        if (shouldDraw)
        {
            node.Draw();
        }

        AddUngroupedNode(node);

        return node;
    }
    #endregion

    #region Callbacks
    private void OnElementDeleted()
    {
        deleteSelection = (operationName, AskUser) =>
        {
            //Creating this secondary list allows the nodes in a selection to be add
            //As deleting them mid operation would alter the list, and could
            //Result in an error being thrown
            Type groupType = typeof(DialogueGroup);
            Type edgeType = typeof(Edge);

            List<DialogueGroup> groupsToDelete = new List<DialogueGroup>();
            List<Edge> edgesToDelete = new List<Edge>();
            List<DialogueNode> nodesToDelete = new List<DialogueNode>();
            foreach (GraphElement element in selection)
            {
                //pattern matching - simply allows for the variable after DialogueNode to be passed as a parameter
                //Otherwise, would need to cast element as a DialogueNode
                if(element is DialogueNode node)
                {
                    nodesToDelete.Add(node);

                    continue;
                }

                if(element.GetType() == edgeType)
                {
                    Edge edge = (Edge)element;

                    edgesToDelete.Add(edge);

                    continue;
                }

                if(element.GetType() != groupType)
                {
                    continue;
                }

                DialogueGroup group = (DialogueGroup)element;

                

                groupsToDelete.Add(group);
            }

            foreach(DialogueGroup group in groupsToDelete)
            {
                List<DialogueNode> groupNodes = new List<DialogueNode>();

                foreach(GraphElement groupElement in group.containedElements)
                {
                    if(!(groupElement is DialogueNode))
                    {
                        continue;
                    }

                    DialogueNode groupNode = (DialogueNode)groupElement;

                    groupNodes.Add(groupNode);
                }

                group.RemoveElements(groupNodes);

                RemoveGroup(group);

                RemoveElement(group);
            }

            DeleteElements(edgesToDelete);

            foreach(DialogueNode node in nodesToDelete)
            {
                if(node.group != null)
                {
                    node.group.RemoveElement(node);
                }
                RemoveUngroupedNode(node);

                node.DisconnectAllPorts();

                RemoveElement(node);
            }
        };
    }

    void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach(GraphElement element in elements)
            {
                if(!(element is DialogueNode))
                {
                    continue;
                }

                DialogueGroup nodeGroup = (DialogueGroup)group;

                DialogueNode node = (DialogueNode)element;

                RemoveUngroupedNode(node);

                AddGroupedNode(node, nodeGroup);
            }
        };
    }

    void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DialogueNode))
                {
                    continue;
                }

                DialogueNode node = (DialogueNode)element;

                RemoveGroupedNode(node, group);

                AddUngroupedNode(node);

            }
        };
    }

    void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DialogueGroup dialogueGroup = (DialogueGroup)group;

            dialogueGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

            if (string.IsNullOrEmpty(dialogueGroup.title))
            {
                if (!string.IsNullOrEmpty(dialogueGroup.oldTitle))
                {
                    RepeatedNamesAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dialogueGroup.oldTitle))
                {
                    RepeatedNamesAmount--;
                }
            }

            RemoveGroup(dialogueGroup);

            dialogueGroup.oldTitle = dialogueGroup.title;

            AddGroup(dialogueGroup);
        };
    }

    void OnGraphViewChanged()
    {
        graphViewChanged = (changes) =>
        {
            if (changes.edgesToCreate != null)
            {
                foreach(Edge edge in changes.edgesToCreate)
                {
                    DialogueNode nextNode = (DialogueNode) edge.input.node;

                    ChoiceSaveData choiceData = (ChoiceSaveData)edge.output.userData;

                    choiceData.NodeID = nextNode.ID;
                }
            }

            if(changes.elementsToRemove != null)
            {
                Type edgeType = typeof(Edge);

                foreach(GraphElement element in changes.elementsToRemove)
                {
                    if(element.GetType() != edgeType)
                    {
                        continue;
                    }

                    Edge edge = (Edge)element;

                    ChoiceSaveData choiceData = (ChoiceSaveData)edge.output.userData;

                    choiceData.NodeID = "";
                }
            }

            return changes;
        };
    }

    #endregion

    #region Repeated Elements

    public void AddUngroupedNode(DialogueNode node) {
        string nodeName = node.DialogueName.ToLower();

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            NodeErrorData nodeErrorData = new NodeErrorData();

            nodeErrorData.Nodes.Add(node);

            ungroupedNodes.Add(nodeName, nodeErrorData);

            return;
        }

        

        List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Add(node);

        //When a new node is created and the name already exists in the dictionary, update the colour

        Color errorColor = ungroupedNodes[nodeName].ErrorData.color;

        node.SetErrorStyle(errorColor);

        if(ungroupedNodesList.Count == 2)
        {
            ++RepeatedNamesAmount;

            ungroupedNodesList[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveUngroupedNode(DialogueNode node)
    {
        string nodeName = node.DialogueName.ToLower();

        List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Remove(node);

        node.ResetStyle();

        if(ungroupedNodesList.Count == 1)
        {
            --RepeatedNamesAmount;
           ungroupedNodesList[0].ResetStyle();
            return;
        }

        if(ungroupedNodesList.Count == 0)
        {
            ungroupedNodes.Remove(nodeName);
        }
    }

    public void AddGroupedNode(DialogueNode node, DialogueGroup group)
    {
        string nodeName = node.DialogueName.ToLower();

        node.group = group;

        if (!groupedNodes.ContainsKey(group))
        {
            groupedNodes.Add(group, new SerializableDictionary<string, NodeErrorData>());
        }

        if (!groupedNodes[group].ContainsKey(nodeName))
        {
            NodeErrorData nodeErrorData = new NodeErrorData();

            nodeErrorData.Nodes.Add(node);

            groupedNodes[group].Add(nodeName, nodeErrorData);


            return;
        }

        List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

        groupedNodesList.Add(node);

        Color errorColor = groupedNodes[group][nodeName].ErrorData.color;

        node.SetErrorStyle(errorColor);

        if(groupedNodesList.Count == 2)
        {
            ++RepeatedNamesAmount;

            groupedNodesList[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveGroupedNode(DialogueNode node, Group group)
    {
        string nodeName = node.DialogueName.ToLower();

        node.group = null;

        List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

        groupedNodesList.Remove(node);

        node.ResetStyle();

        if (groupedNodesList.Count == 1)
        {
            --RepeatedNamesAmount;
            groupedNodesList[0].ResetStyle();

            return;
        }

        if (groupedNodesList.Count == 0)
        {
            groupedNodes[group].Remove(nodeName);

            if (groupedNodes[group].Count == 0)
            {
                groupedNodes.Remove(group);
            }
        }
    }

    public void AddGroup(DialogueGroup group)
    {
        string groupName = group.title.ToLower();
        if (!groups.ContainsKey(groupName))
        {
            GroupErrorData groupErrorData = new GroupErrorData();

            groupErrorData.Groups.Add(group);

            groups.Add(groupName, groupErrorData);

            return;
        }

        List<DialogueGroup> groupsList = groups[groupName].Groups;

        groupsList.Add(group);

        Color errorColor = groups[groupName].ErrorData.color;

        group.SetErrorStyle(errorColor);

        if(groupsList.Count == 2)
        {
            ++RepeatedNamesAmount;
            groupsList[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveGroup(DialogueGroup group)
    {
        string oldGroupName = group.oldTitle.ToLower();

        List<DialogueGroup> groupList = groups[oldGroupName].Groups;

        groupList.Remove(group);

        group.ResetStyle();

        if(groupList.Count == 1)
        {
            groupList[0].ResetStyle();

            return;
        }

        if(groupList.Count == 0)
        {
            --RepeatedNamesAmount;

            groups.Remove(oldGroupName);
        }
    }

    #endregion

    #region Utilities
    public Vector2 getLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition -= editorWindow.position.position;
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }

    public void clearGraph()
    {
        graphElements.ForEach(graphElement => RemoveElement(graphElement));

        groups.Clear();
        groupedNodes.Clear();
        ungroupedNodes.Clear();

        repeatedNamesAmount = 0;
    }

    public void ToggleMiniMap()
    {
        miniMap.visible = !miniMap.visible;
    }
    #endregion
}
