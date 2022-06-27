using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public static class DialogueStyleUtility
{
    public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
    {
        foreach(string className in classNames)
        {
            element.AddToClassList(className);
        }

        return element;
    }

    public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
    {
        foreach(string styleSheetName in styleSheetNames)
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(styleSheetName);

            element.styleSheets.Add(styleSheet);
        }

        return element;
    }
}
