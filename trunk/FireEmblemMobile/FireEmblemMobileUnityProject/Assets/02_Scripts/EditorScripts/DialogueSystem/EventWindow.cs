using System;
using System.Collections.Generic;
using System.IO;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.Dialog.DialogSystem;
using Game.GameActors.Units;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class EventWindow: EditorWindow
    {

        private readonly string defaultFileName = "DialoguesFileName";
        private static TextField fileNameTextField;
        private Button saveButton;
        private LGGraphView graphView;
        private Button miniMapButton;
        private string lastGraphName;
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
            if(lastGraphName!=null)
                Load($"Assets/02_Scripts/EditorScripts/DialogueSystem/Graphs/{lastGraphName}.asset");
        }

        void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = ElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
            saveButton = ElementUtility.CreateButton("Save", ()=> Save());
            Button loadButton = ElementUtility.CreateButton("Load", () => Load());
            Button clearButton = ElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = ElementUtility.CreateButton("Reset", () => ResetGraph());
            miniMapButton = ElementUtility.CreateButton("MiniMap", () => ToogleMiniMap());
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(miniMapButton);
            toolbar.AddStyleSheets("DialogueSystem/LGToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

        private void ToogleMiniMap()
        {  
            graphView.ToggleMiniMap();
            miniMapButton.ToggleInClassList("lg-toolbar__button__selected");
        }


        private void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            Clear();
            IOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            IOUtility.Load();
            lastGraphName = Path.GetFileNameWithoutExtension(filePath);
        }
        private void Load()
        {
            string filePath=EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/02_Scripts/EditorScripts/DialogueSystem/Graphs", "asset");
           Load(filePath);
        }
        private void Clear()
        {
            graphView.ClearGraph();
        }
        private void ResetGraph()
        {
            graphView.ClearGraph();
            UpdateFileName(defaultFileName);
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
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