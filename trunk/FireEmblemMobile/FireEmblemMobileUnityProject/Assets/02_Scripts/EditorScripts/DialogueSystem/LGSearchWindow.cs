using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    public class LGSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private LGGraphView graphView;
        private Texture2D indentationIcon;
        public void Initialize(LGGraphView graphView)
        {
            this.graphView = graphView;
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0,0, Color.clear);
            indentationIcon.Apply();
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon))
                {
                    level = 2,
                    userData = DialogType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multi Choice", indentationIcon))
                {
                    level = 2,
                    userData = DialogType.MultiChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePos = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DialogType.SingleChoice:
                    SingleChoiceNode node = (SingleChoiceNode)graphView.CreateNode(DialogType.SingleChoice, localMousePos);
                    graphView.AddElement(node);
                    return true; 
                case DialogType.MultiChoice: 
                    MultiChoiceNode multiNode = (MultiChoiceNode)graphView.CreateNode(DialogType.MultiChoice, localMousePos);
                    graphView.AddElement(multiNode);
                    return true;
                case Group unusedGroup:
                    Group group = graphView.CreateGroup("DialogueGroup", localMousePos);
                    graphView.AddElement(group);
                    return true;
                default: return false;
            }
        }
    }
}