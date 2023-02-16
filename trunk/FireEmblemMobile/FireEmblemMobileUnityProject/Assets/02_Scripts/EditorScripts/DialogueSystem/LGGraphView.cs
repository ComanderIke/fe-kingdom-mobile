using System;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.EditorScripts.DialogueSystem.Data.Error;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using _02_Scripts.Game.GUI.Utility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace __2___Scripts.External.Editor
{
    public class LGGraphView:GraphView
    {
        private EventWindow editorWindow;
        private LGSearchWindow lgSearchWindow;
        private MiniMap miniMap;
        private SerializableDictionary<string, LGNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, LGGroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<string, LGNodeErrorData>> groupedNodes;

        private int nameErrorsAmount;

        public int NameErrorsAmount
        {
            get
            {
                return nameErrorsAmount;
            }
            set
            {
                nameErrorsAmount = value;
                if (nameErrorsAmount == 0)
                {
                    editorWindow.EnableSaving();
                }

                if (nameErrorsAmount == 1)
                {
                    editorWindow.DisableSaving();
                }
            }
        }
        
        public LGGraphView(EventWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            ungroupedNodes = new SerializableDictionary<string, LGNodeErrorData>();
            groups =new SerializableDictionary<string, LGGroupErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, LGNodeErrorData>>();
            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddGridBackground();
            OnElementsDeleted();
            OnGroupRenamed();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGraphViewChanged();
            AddStyles();
            AddMinimapStyles();

        }

        private void AddMinimapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color(0.12f, 0.12f, 0.13f, 255));
            StyleColor borderColor = new StyleColor(new Color(0.22f, 0.22f, 0.22f, 255));
            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
            miniMap.style.borderRightColor = borderColor;


        }

        public void ToggleMiniMap()
        {
            miniMap.visible = !miniMap.visible;
        }

        private void AddMinimap()
        {
            miniMap = new MiniMap()
            {
                anchored = true
            };
            miniMap.SetPosition(new Rect(15,50,200,180));
            Add(miniMap);
            miniMap.visible = false;
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
            this.AddManipulator(CreateNodeContextualMenu("Add Fight Node",DialogType.Fight));
            this.AddManipulator(CreateNodeContextualMenu("Add Battle Node",DialogType.Battle));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Choice Event Node",DialogType.MultiChoiceEvent));
        
            this.AddManipulator(CreateGroupContextualMenu());
        }
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator man = new ContextualMenuManipulator(menuEvent =>
                menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))));
            return man;
        }

        public DialogGroup CreateGroup(string title, Vector2 localMousePos)
        {
            DialogGroup group = new DialogGroup(title, localMousePos);
            AddGroup(group);
            AddElement(group);
            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is DialogNode))
                {
                    continue;
                }

                DialogNode node = (DialogNode)selectedElement;
                group.AddElement(node);
            }
            return group;
        }
        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogType type)
        {
            ContextualMenuManipulator man = new ContextualMenuManipulator(menuEvent =>
                menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName",type,GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
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

        public DialogNode CreateNode(string nodeName, DialogType type, Vector2 position, bool shouldDraw =true)
        {
            Type nodeType = Type.GetType($"_02_Scripts.EditorScripts.DialogueSystem.Elements.{type}Node");
            DialogNode node = (DialogNode)Activator.CreateInstance(nodeType);
            node.Initialize(nodeName,this,position);
            if(shouldDraw)
                node.Draw();
            AddUngroupedNode(node);
            return node;
        }

        private void AddGroup(DialogGroup group)
        {
            string groupName = group.title.ToLower();
            if (!groups.ContainsKey(groupName))
            {
                LGGroupErrorData groupErrorData = new LGGroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupName, groupErrorData);
                return;
            }

            List<DialogGroup> groupsList = groups[groupName].Groups;
            groups[groupName].Groups.Add(group);
            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);
            if (groupsList.Count == 2)
            {
                ++NameErrorsAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        
        public void AddGroupedNode(DialogNode node, DialogGroup group)
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = group;
            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, LGNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                LGNodeErrorData nodeErrorData = new LGNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                return;
            }

            List<DialogNode> groupNodesList = groupedNodes[group][nodeName].Nodes;
            groupNodesList.Add(node);
            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);
            if (groupNodesList.Count == 2)
            {
                ++NameErrorsAmount;
                groupNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void AddUngroupedNode(DialogNode node)
        {
            string nodeName = node.DialogueName.ToLower();
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
                ++NameErrorsAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DialogNode node)
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = null;
            List<DialogNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Remove(node);
            node.ResetStyle();
            if (ungroupedNodesList.Count == 1)
            {
                --NameErrorsAmount;
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
                Type groupType = typeof(DialogGroup);
                Type edgeType = typeof(Edge);
                List<DialogGroup> groupsToDelete = new List<DialogGroup>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DialogNode> nodesToDelete = new List<DialogNode>();
               
                foreach (GraphElement element in selection)
                {
                    if (element is DialogNode node)
                    {
                      nodesToDelete.Add(node);   
                      continue;
                    }
                    if (element.GetType() == edgeType)
                    {
                        Edge edge = (Edge)element;
                        edgesToDelete.Add(edge);
                        continue;
                    }
                    if (element.GetType() != groupType)
                    {
                        continue;
                    }

                    DialogGroup group = (DialogGroup)element;
                    groupsToDelete.Add(group);
                }

                foreach (DialogGroup group in groupsToDelete)
                {
                    List<DialogNode> groupNodes = new List<DialogNode>();
                    foreach(GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is DialogNode))
                        {
                            continue;
                        }

                        DialogNode groupNode = (DialogNode)groupElement;
                        groupNodes.Add(groupNode);
                    }
                    group.RemoveElements(groupNodes);
                    RemoveGroup(group);

                    RemoveElement(group);
                }
                DeleteElements(edgesToDelete);
                foreach (DialogNode node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);
                    node.DisconnectAllPorts();
                    RemoveElement(node);
                }
            };
        }

        private void RemoveGroup(DialogGroup group)
        {
            string oldGroupName = group.oldTitle.ToLower();
            List<DialogGroup> groupsList = groups[oldGroupName].Groups;
            groupsList.Remove(group);
            group.ResetStyle();
            if (groupsList.Count == 1)
            {
                --NameErrorsAmount;
                groupsList[0].ResetStyle();
                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DialogNode nextNode = (DialogNode)edge.input.node;
                        LGChoiceSaveData choiceData = (LGChoiceSaveData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge)element;
                        LGChoiceSaveData choiceData= (LGChoiceSaveData)edge.output.userData;
                        choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DialogGroup dialogGroup = (DialogGroup)group;
                dialogGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                if (string.IsNullOrEmpty(dialogGroup.title))
                {
                    if (!string.IsNullOrEmpty(dialogGroup.oldTitle))
                    {
                        ++NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dialogGroup.oldTitle))
                    {
                        --NameErrorsAmount;
                    }
                }
                RemoveGroup(dialogGroup);
                dialogGroup.oldTitle =  dialogGroup.title ;
                AddGroup(dialogGroup);
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
                    DialogGroup nodeGroup = (DialogGroup)group;
                    DialogNode node = (DialogNode)element;
                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
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

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            
            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();
            NameErrorsAmount = 0;
        }
        public void RemoveGroupedNode(DialogNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();
            List<DialogNode> groupNodesList = groupedNodes[group][nodeName].Nodes;
            groupNodesList.Remove(node);
            node.ResetStyle();
            if (groupNodesList.Count == 1)
            {
                --NameErrorsAmount;
                groupNodesList[0].ResetStyle();
            }
            else if (groupNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);
                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }
    }
}