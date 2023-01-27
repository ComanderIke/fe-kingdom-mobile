using System;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.EditorScripts.DialogueSystem.Data.Error;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
    public class LGGraphView:GraphView
    {
        private EventWindow editorWindow;
        private LGSearchWindow lgSearchWindow;
        private SerializableDictionary<string, LGNodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, LGNodeErrorData>> groupNodes;
        
        public LGGraphView(EventWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            ungroupedNodes = new SerializableDictionary<string, LGNodeErrorData>();
            groupNodes = new SerializableDictionary<Group, SerializableDictionary<string, LGNodeErrorData>>();
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            AddStyles();
            
        }

        private void AddSearchWindow()
        {
            if (lgSearchWindow == null)
            {
                lgSearchWindow = ScriptableObject.CreateInstance<LGSearchWindow>();
                lgSearchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), lgSearchWindow);
        }

        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        
            this.AddManipulator(new ContentDragger());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Single Choice Node",DialogType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Choice Node",DialogType.MultiChoice));
            this.AddManipulator(CreateGroupContextualMenu());
        }
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator man = new ContextualMenuManipulator(menuEvent =>
                menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
            return man;
        }

        public Group CreateGroup(string title, Vector2 localMousePos)
        {
            Group group = new Group()
            {
                title=title
            };
            group.SetPosition(new Rect(localMousePos, Vector2.zero));
            return group;
        }
        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogType type)
        {
            ContextualMenuManipulator man = new ContextualMenuManipulator(menuEvent =>
                menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(type,GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
            return man;
        }
        void AddGridBackground()
        {
            GridBackground grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0,grid);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                    return;
                if (startPort.node == port.node)
                    return;
                if (startPort.direction == port.direction)
                    return;
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        public DialogNode CreateNode(DialogType type, Vector2 position)
        {
            Type nodeType = Type.GetType($"_02_Scripts.EditorScripts.DialogueSystem.Elements.{type}Node");
            DialogNode node = (DialogNode)Activator.CreateInstance(nodeType);
            node.Initialize(this,position);
            node.Draw();
            AddUngroupedNode(node);
            return node;
        }

        public void AddGroupedNode(DialogNode node, Group group)
        {
            string nodeName = node.DialogueName;
            node.Group = group;
            if (!groupNodes.ContainsKey(group))
            {
                groupNodes.Add(group, new SerializableDictionary<string, LGNodeErrorData>());
            }

            if (!groupNodes[group].ContainsKey(nodeName))
            {
                LGNodeErrorData nodeErrorData = new LGNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupNodes[group].Add(nodeName, nodeErrorData);
                return;
            }

            List<DialogNode> groupNodesList = groupNodes[group][nodeName].Nodes;
            groupNodesList.Add(node);
            Color errorColor = groupNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);
            if (groupNodesList.Count == 2)
            {
                groupNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void AddUngroupedNode(DialogNode node)
        {
            string nodeName = node.DialogueName;
            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                LGNodeErrorData nodeErrorData = new LGNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }

            List<DialogNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Add(node);
            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);
            if (ungroupedNodesList.Count == 2)
            {
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DialogNode node)
        {
            string nodeName = node.DialogueName;
            node.Group = null;
            List<DialogNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Remove(node);
            node.ResetStyle();
            if (ungroupedNodesList.Count == 1)
            {
                ungroupedNodesList[0].ResetStyle();
            }
            else if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }

            
        }
        void AddStyles()
        {
            this.AddStyleSheets("DialogueSystem/GraphViewStyles.uss"
            ,"DialogueSystem/NodeStyles.uss");
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow=false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<DialogNode> nodesToDelete = new List<DialogNode>();
                foreach (GraphElement element in selection)
                {
                    if (element is DialogNode node)
                    {
                      nodesToDelete.Add(node);   
                      continue;
                    }
                }

                foreach (DialogNode node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if(!(element is DialogNode))
                        continue;
                    DialogNode node = (DialogNode)element;
                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, group);
                }
            };
        }
        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if(!(element is DialogNode))
                        continue;
                    DialogNode node = (DialogNode)element;
                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);

                }
            };
        }

        public void RemoveGroupedNode(DialogNode node, Group group)
        {
            string nodeName = node.DialogueName;
            List<DialogNode> groupNodesList = groupNodes[group][nodeName].Nodes;
            groupNodesList.Remove(node);
            node.ResetStyle();
            if (groupNodesList.Count == 1)
            {
                groupNodesList[0].ResetStyle();
            }
            else if (groupNodesList.Count == 0)
            {
                groupNodes[group].Remove(nodeName);
                if (groupNodes[group].Count == 0)
                {
                    groupNodes.Remove(group);
                }
            }
        }
    }
}