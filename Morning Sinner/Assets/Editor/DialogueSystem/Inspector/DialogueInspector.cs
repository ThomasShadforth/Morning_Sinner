using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialogue))]
public class DialogueInspector : Editor
{
    //Custom Editor tells class what runtime it is for

    private SerializedProperty dialogueContainerProperty;
    private SerializedProperty dialogueGroupProperty;
    private SerializedProperty dialogueProperty;

    private SerializedProperty groupedDialoguesProperty;
    private SerializedProperty startingDialoguesOnlyProperty;

    private void OnEnable()
    {
        dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
        dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
        dialogueProperty = serializedObject.FindProperty("dialogue");
        groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
        startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");
    }

    public override void OnInspectorGUI()
    {
        DrawDialogueContainerArea();

        DrawFiltersArea();

        DrawDialogueGroupArea();
    }

    



    #region Draw Methods
    private void DrawDialogueContainerArea()
    {
        EditorGUILayout.LabelField("Dialogue Container", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(dialogueContainerProperty);
    }

    private void DrawFiltersArea()
    {
        EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(groupedDialoguesProperty);
        EditorGUILayout.PropertyField(startingDialoguesOnlyProperty);
    }

    private void DrawDialogueGroupArea()
    {
        EditorGUILayout.LabelField("Dialogue Group", EditorStyles.boldLabel);

        //EditorGUILayout.Popup("Dialogue Group");

        EditorGUILayout.PropertyField(dialogueGroupProperty);
    }
    #endregion
}
