using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialogue))]
public class DialogueInspector : Editor
{
    //Custom Editor tells class what runtime it is for

    //serialized object is a representation of the selected object class data
    //Serialised properties hold data related to a property (Variable), such as type, so it can send the correct value when needed
    //Values are not updated in the inspector because they are merely a 'representation' of the data, the changes aren't synchronised by default
    private SerializedProperty dialogueContainerProperty;
    private SerializedProperty dialogueGroupProperty;
    private SerializedProperty dialogueProperty;

    private SerializedProperty groupedDialoguesProperty;
    private SerializedProperty startingDialoguesOnlyProperty;

    private SerializedProperty selectedDialogueGroupIndexProperty;
    private SerializedProperty selectedDialogueIndexProperty;

    private void OnEnable()
    {
        //Finds the property with the name specified, and sets it as the value of the properties
        dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
        dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
        dialogueProperty = serializedObject.FindProperty("dialogue");
        groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
        startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

        selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
        selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDialogueContainerArea();

        DialogueContainerSO dialogueContainer = (DialogueContainerSO)dialogueContainerProperty.objectReferenceValue;

        //Reference value is received from object selected in the inspector
        if(dialogueContainer == null)
        {
            StopDrawing("Select a Dialogue Container to see the rest of the Inspector");

            return;
        }

        DrawFiltersArea();

        bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;


        List<string> DialogueNames;

        string DialogueFolderPath = $"Assets/DialogueSystem/Dialogues/{dialogueContainer.FileName}";

        string DialogueInfoMessage;

        if (groupedDialoguesProperty.boolValue)
        {
            List<string> dialogueGroupNames = dialogueContainer.GetDialogueGroupNames();

            if(dialogueGroupNames.Count == 0)
            {
                StopDrawing("There are no Dialogue Groups in this Dialogue Container");
                return;
            }

            DrawDialogueGroupArea(dialogueContainer, dialogueGroupNames);

            DialogueGroupSO dialogueGroup = (DialogueGroupSO)dialogueGroupProperty.objectReferenceValue;

            DialogueNames = dialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

            DialogueFolderPath += $"Groups/{dialogueGroup.GroupName}/Dialogues";

            DialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? "Starting" : "") + "Dialogues in this Dialogue Group";
        }
        else
        {
            DialogueNames = dialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);

            DialogueFolderPath += "/Global/Dialogues";

            DialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? "Starting" : "") + "Ungrouped Dialogues in this Dialogue Container";
        }

        if(DialogueNames.Count == 0)
        {
            StopDrawing(DialogueInfoMessage);

            return;
        }


        DrawDialogueArea(DialogueNames, DialogueFolderPath);

        serializedObject.ApplyModifiedProperties();

        
    }

    

    #region Draw Methods
    private void DrawDialogueContainerArea()
    {
        DialogueInspectorUtility.DrawHeader("Dialogue Container");

        dialogueContainerProperty.DrawPropertyField();

        DialogueInspectorUtility.DrawSpace();
    }

    private void DrawFiltersArea()
    {
        DialogueInspectorUtility.DrawHeader("Filters");

        groupedDialoguesProperty.DrawPropertyField();
        startingDialoguesOnlyProperty.DrawPropertyField();

        DialogueInspectorUtility.DrawSpace();
    }

    private void DrawDialogueGroupArea(DialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
    {
        DialogueInspectorUtility.DrawHeader("Dialogue Group");

        int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

        DialogueGroupSO oldDialogueGroup = (DialogueGroupSO)dialogueGroupProperty.objectReferenceValue;

        bool isOldDialogueGroupNull = oldDialogueGroup == null;

        string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

        UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

        selectedDialogueGroupIndexProperty.intValue = DialogueInspectorUtility.DrawPopUp("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

        string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];
        DialogueGroupSO selectedDialogueGroup = DialogueIOUtility.LoadAsset<DialogueGroupSO>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

        dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

        DialogueInspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

        DialogueInspectorUtility.DrawSpace();
    }

    
    private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
    {
        DialogueInspectorUtility.DrawHeader("Dialogue");

        int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

        DialogueSO oldDialogue = (DialogueSO)dialogueProperty.objectReferenceValue;

        bool isOldDialogueNull = oldDialogue == null;

        string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

        UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

        selectedDialogueIndexProperty.intValue = DialogueInspectorUtility.DrawPopUp("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

        string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

        DialogueSO selectedDialogue = DialogueIOUtility.LoadAsset<DialogueSO>(dialogueFolderPath, selectedDialogueName);

        dialogueProperty.objectReferenceValue = selectedDialogue;

        DialogueInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
    }

    private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
    {
        DialogueInspectorUtility.DrawHelpBox(reason, messageType);
        DialogueInspectorUtility.DrawSpace();
        DialogueInspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Index Methods
    private void UpdateIndexOnNamesListUpdate(List<string> OptionNames,SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
    {
        if (isOldPropertyNull)
        {
            indexProperty.intValue = 0;

            return;
        }

        bool OldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > OptionNames.Count - 1;
        bool OldNameIsDifferentThanSelectedName = OldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != OptionNames[oldSelectedPropertyIndex];
        
        if (OldNameIsDifferentThanSelectedName)
        {
            if (OptionNames.Contains(oldPropertyName))
            {
                indexProperty.intValue = OptionNames.IndexOf(oldPropertyName);
            }
            else
            {
                indexProperty.intValue = 0;
            }
        }
        
    }

    #endregion
}
