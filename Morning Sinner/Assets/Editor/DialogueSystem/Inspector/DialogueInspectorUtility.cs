using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DialogueInspectorUtility
{
    public static void DrawDisabledFields(Action action)
    {
        EditorGUI.BeginDisabledGroup(true);
        action.Invoke();
        EditorGUI.EndDisabledGroup();
    }

    public static void DrawHeader(string label)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

    public static void DrawHelpBox(string Message, MessageType messageType = MessageType.Info, bool wide = true)
    {
        EditorGUILayout.HelpBox(Message, messageType, wide);
    }

    public static void DrawPropertyField(this SerializedProperty serializedProperty)
    {
        EditorGUILayout.PropertyField(serializedProperty);
    }

    public static int DrawPopUp(string label, SerializedProperty selectedIndexProperty, string[] options)
    {
        return EditorGUILayout.Popup(label, selectedIndexProperty.intValue, options);
    }

    public static void DrawSpace(int amount = 4)
    {
        EditorGUILayout.Space(amount);
    }
}
