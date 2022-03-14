using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueEditor : EditorWindow
{
    private readonly string defaultFileName = "DialogueFileName";
    private TextField fileNameTextField;
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

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);

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
    #endregion

    #region Utility Methods
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