using System;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class EventWindow: EditorWindow
    {

        private readonly string defaultFileName = "DialoguesFileName";
        private TextField fileNameTextField;
        private Button saveButton;
        private LGGraphView graphView;
        public EventWindow()
        {
            
        }
        [MenuItem("Tools/EventWindow")]
        public static void ShowMyEditor()
        {
            GetWindow<EventWindow>("EventWindow");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = ElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
            saveButton = ElementUtility.CreateButton("Save", ()=> Save());
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.AddStyleSheets("DialogueSystem/LGToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is valid.", "OK");
                return;
            }
            IOUtility.Initialize(graphView, fileNameTextField.value);
            IOUtility.Save();
        }

        void AddGraphView()
        {
            graphView = new LGGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/GraphViewVariables.uss");
        }

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }
        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }
    }
}