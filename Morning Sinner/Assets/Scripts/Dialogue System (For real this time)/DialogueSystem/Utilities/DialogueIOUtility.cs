using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class DialogueIOUtility
{
    private static string graphFileName;
    private static string containerFolderPath;
    private static DialogueGraphView _graphview;

    private static List<DialogueGroup> groups;
    private static List<DialogueNode> nodes;

    private static Dictionary<string, DialogueGroupSO> createdGroups;
    private static Dictionary<string, DialogueSO> createdDialogues;
    public static void Initialize(DialogueGraphView graphView,string graphName)
    {
        _graphview = graphView;
        graphFileName = graphName;
        containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";

        groups = new List<DialogueGroup>();
        nodes = new List<DialogueNode>();
        createdGroups = new Dictionary<string, DialogueGroupSO>();
        createdDialogues = new Dictionary<string, DialogueSO>();
    }

    #region Save Methods
    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();

        GraphSaveDataSO graphData = CreateAsset<GraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");

        graphData.Initialize(graphFileName);

        DialogueContainerSO dialogueContainer = CreateAsset<DialogueContainerSO>(containerFolderPath, graphFileName);
        dialogueContainer.Initialize(graphFileName);

        SaveGroups(graphData, dialogueContainer);
        SaveNodes(graphData, dialogueContainer);

        SaveAsset(graphData);
        SaveAsset(dialogueContainer);
    }

    

    #region Groups
    private static void SaveGroups(GraphSaveDataSO graphData, DialogueContainerSO dialogueContainer)
    {
        List<string> groupNames = new List<string>();
        foreach(DialogueGroup group in groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
    }
    private static void SaveGroupToGraph(DialogueGroup group, GraphSaveDataSO graphData)
    {
        GroupSaveData groupData = new GroupSaveData()
        {
            ID = group.ID,
            GroupName = group.title,
            Position = group.GetPosition().position
        };

        graphData.Groups.Add(groupData);
    }
    private static void SaveGroupToScriptableObject(DialogueGroup group, DialogueContainerSO dialogueContainer)
    {
        string groupName = group.title;
        CreateFolder($"{containerFolderPath}/Groups", groupName);
        CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

        DialogueGroupSO dialogueGroup = CreateAsset<DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
        dialogueGroup.Initialize(groupName);

        createdGroups.Add(group.ID, dialogueGroup);

        dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DialogueSO>());

        SaveAsset(dialogueGroup);
    }
    private static void UpdateOldGroups(List<string> currentGroupNames, GraphSaveDataSO graphData)
    {
        if(graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

            foreach(string groupToRemove in groupsToRemove)
            {
                RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
            }
        }

        graphData.OldGroupNames = new List<string>(currentGroupNames);
    }
    #endregion
    #region Nodes
    private static void SaveNodes(GraphSaveDataSO graphData, DialogueContainerSO dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();

        List<string> ungroupedNodeNames = new List<string>();
        foreach (DialogueNode node in nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);

            if(node.group != null)
            {
                groupedNodeNames.AddItem(node.group.title, node.DialogueName);
                continue;
            }

            ungroupedNodeNames.Add(node.DialogueName);
        }

        UpdateDialogueChoicesConnections();

        UpdateOldGroupedNodes(groupedNodeNames, graphData);

        UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
    }

    private static void SaveNodeToGraph(DialogueNode node, GraphSaveDataSO graphData)
    {
        List<ChoiceSaveData> choices = new List<ChoiceSaveData>();

        foreach(ChoiceSaveData choice in node.Choices)
        {
            ChoiceSaveData choiceData = new ChoiceSaveData()
            {
                Text = choice.Text,
                NodeID = choice.NodeID
            };

            choices.Add(choiceData);
        }

        DialogueNodeSaveData nodeData = new DialogueNodeSaveData()
        {
            ID = node.ID,
            Name = node.DialogueName,
            Choices = choices,
            Text = node.Text,
            GroupID = node.group?.ID,
            dialogueType = node.dialogueType,
            Position = node.GetPosition().position
        };

        graphData.Nodes.Add(nodeData);
    }

    private static void SaveNodeToScriptableObject(DialogueNode node, DialogueContainerSO dialogueContainer)
    {
        DialogueSO dialogue;

        if(node.group != null)
        {
            dialogue = CreateAsset<DialogueSO>($"{containerFolderPath}/Groups/{node.group.title}/Dialogues", node.DialogueName);

            dialogueContainer.DialogueGroups.AddItem(createdGroups[node.group.ID], dialogue);
        }
        else
        {
            dialogue = CreateAsset<DialogueSO>($"{containerFolderPath}/Global/Dialgoues", node.DialogueName);

            dialogueContainer.UngroupedDialogues.Add(dialogue);
        }

        dialogue.Initialize(
            node.DialogueName,
            node.Text,
            ConvertNodeChoicesToDialogueChoices(node.Choices),
            node.dialogueType,
            node.isStartingNode()
            );
        createdDialogues.Add(node.ID, dialogue);

        SaveAsset(dialogue);
    }

    private static List<DialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<ChoiceSaveData> nodeChoices)
    {
        List<DialogueChoiceData> dialogueChoices = new List<DialogueChoiceData>();

        foreach(ChoiceSaveData nodeChoice in nodeChoices)
        {
            DialogueChoiceData choiceData = new DialogueChoiceData()
            {
                Text = nodeChoice.Text
            };

            dialogueChoices.Add(choiceData);
        }

        return dialogueChoices;
    }

    private static void UpdateDialogueChoicesConnections()
    {
        foreach(DialogueNode node in nodes)
        {
            DialogueSO dialogue = createdDialogues[node.ID];

            for(int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
            {
                ChoiceSaveData nodeChoice = node.Choices[choiceIndex];

                if (string.IsNullOrEmpty(nodeChoice.NodeID))
                {
                    continue;
                }

                dialogue.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];

                SaveAsset(dialogue);
            }
        }
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, GraphSaveDataSO graphData)
    {
        if(graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
        {
            foreach(KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new List<string>();

                if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                {
                    nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                }

                foreach(string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                }
            }
        }

        graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
    }

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, GraphSaveDataSO graphData)
    {
        if(graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
        {
            List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach(string nodeToRemove in nodesToRemove)
            {
                RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
            }
        }

        graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
    }

    

    #endregion

    #endregion

    #region Creation Methods
    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/Editor/DialogueSystem", "Graphs");
        CreateFolder("Assets", "DialogueSystem");
        CreateFolder("Assets/DialogueSystem", "Dialogues");

        CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
        CreateFolder(containerFolderPath, "Global");
        CreateFolder(containerFolderPath, "Groups");
        CreateFolder($"{containerFolderPath}/Global", "Dialogues");
    }
    #endregion

    #region Fetch Methods

    private static void GetElementsFromGraphView()
    {
        Type groupType = typeof(DialogueGroup);
        _graphview.graphElements.ForEach(graphElement =>
        {
            if(graphElement is DialogueNode node)
            {
                nodes.Add(node);

                return;
            }

            if(graphElement.GetType() == groupType)
            {
                DialogueGroup group = (DialogueGroup)graphElement;

                groups.Add(group);

                return;
            }
        });
    }
    #endregion

    #region Utility Methods
    private static void CreateFolder(string path, string folderName)
    {
        if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        {
            return;
        }

        AssetDatabase.CreateFolder(path, folderName);
    }

    private static void RemoveFolder(string fullPath)
    {
        FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
        FileUtil.DeleteFileOrDirectory($"{fullPath}/");
    }

    private static T CreateAsset<T>(string path, string assetName) where T: ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

        if(asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        

        return asset;
    }

    private static void RemoveAsset(string path, string assetName)
    {
        AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endregion
}
