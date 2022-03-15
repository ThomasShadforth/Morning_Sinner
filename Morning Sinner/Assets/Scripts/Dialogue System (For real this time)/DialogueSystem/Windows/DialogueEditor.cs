using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

public class DialogueEditor : EditorWindow
{
    private readonly string defaultFileName = "DialogueFileName";
    private static TextField fileNameTextField;
    private Button saveButton;
    private DialogueGraphView _graphview;

    [MenuItem("Window/Dialogue System/Dialogue Graph")]
    public static void ShowExample()
    {
        GetWindow<DialogueEditor>("Dialogue Graph");
        
    }

    private void CreateGUI()
    {
        AddGraphView();
        AddToolbar();
        AddStyles();
    }

    

    #region Elements Addition
    void AddGraphView()
    {
        _graphview = new DialogueGraphView(this);

        //Graph view size is 0 by default, won't see
        //Stretch to parent size stretches it to the width and height of the editor window,
        //even when resizing
        _graphview.StretchToParentSize();

        

        rootVisualElement.Add(_graphview);
        
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        fileNameTextField = ElementUtilities.CreateTextArea(defaultFileName, "File Name:", callback => {
            fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        saveButton = ElementUtilities.CreateButton("Save", () => Save());

        Button loadButton = ElementUtilities.CreateButton("Load", () => Load());
        Button clearButton = ElementUtilities.CreateButton("Clear", () => Clear());
        Button resetButton = ElementUtilities.CreateButton("Reset", () => ResetGraph());
        Button miniMapButton = ElementUtilities.CreateButton("MiniMap", () => ToggleMiniMap());

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);
        toolbar.Add(miniMapButton);

        toolbar.AddStyleSheets("DialogueToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DialogueVariables.uss");
        
    }
    #endregion

    #region Toolbar Actions
    private void Save()
    {
        if (string.IsNullOrEmpty(fileNameTextField.value))
        {
            EditorUtility.DisplayDialog(
                "Invalid File Name",
                "Please ensure the file name you've typed in is valid",
                "Ok"
                );
            return;
        }
        DialogueIOUtility.Initialize(_graphview, fileNameTextField.value);
        DialogueIOUtility.Save();
    }

    private void Load()
    {
        string filePath = EditorUtility.OpenFilePanel("Load Dialogue Graph", "Assets/Editor/DialogueSystem/Graphs", "asset");

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        Clear();

        DialogueIOUtility.Initialize(_graphview, Path.GetFileNameWithoutExtension(filePath));
        DialogueIOUtility.Load();
    }

    private void Clear()
    {
        _graphview.clearGraph();
    }

    private void ResetGraph()
    {
        Clear();
        UpdateFileName(defaultFileName);
    }

    void ToggleMiniMap()
    {
        _graphview.ToggleMiniMap();
    }

    #endregion

    #region Utility Methods
    public static void UpdateFileName(string newFileName)
    {
        fileNameTextField.value = newFileName;
    }
    public void EnableSaving()
    {
        saveButton.SetEnabled(true);
    }

    public void DisableSaving()
    {
        saveButton.SetEnabled(false);
    }
    #endregion
}