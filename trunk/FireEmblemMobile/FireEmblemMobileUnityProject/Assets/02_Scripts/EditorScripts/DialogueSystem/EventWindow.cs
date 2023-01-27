using System;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Utility;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class EventWindow: EditorWindow
    {


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
            AddStyles();
        }

        void AddGraphView()
        {
            LGGraphView graphView = new LGGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/GraphViewVariables.uss");
        }
    }
}