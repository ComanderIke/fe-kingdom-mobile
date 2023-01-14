using System;
using __2___Scripts.External.Editor.Elements;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
    public class LGGraphView:GraphView
    {
        public LGGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
            
        }

        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(CreateNodeContextualMenu("Add Single Choice Node",DialogType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Choice Node",DialogType.MultiChoice));
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogType type)
        {
            ContextualMenuManipulator man = new ContextualMenuManipulator(menuEvent =>
                menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(type,actionEvent.eventInfo.localMousePosition))));
            return man;
        }
        void AddGridBackground()
        {
            GridBackground grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0,grid);
        }

        DialogNode CreateNode(DialogType type, Vector2 position)
        {
            Type nodeType = Type.GetType($"_02_Scripts.EditorScripts.DialogueSystem.Elements.{type}Node");
            DialogNode node = (DialogNode)Activator.CreateInstance(nodeType);
            node.Initialize(position);
            node.Draw();
            return node;
        }
        void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/GraphViewStyles.uss");
            styleSheets.Add(styleSheet);
            styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/NodeStyles.uss");
            styleSheets.Add(styleSheet);
        }
    }
}