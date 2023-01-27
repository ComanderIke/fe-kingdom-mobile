using System.Collections.Generic;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class DialogNode:Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public Group Group { get; set; }
        public DialogType DialogType { get; set; }
        private Color defaultBackgroundColor;
        private LGGraphView graphView;

        public virtual void Initialize(LGGraphView graphView,Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "DialogueText";
            this.graphView = graphView;
            defaultBackgroundColor = new Color(29f/255f, 29/255f, 30/255f);
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddToClassList("node_main-container");
            extensionContainer.AddToClassList("node_extension-container");
        }

        public virtual void Draw()
        {
            TextField dialogueNameTextField = ElementUtility.CreateTextField(DialogueName, callback =>
            {
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = callback.newValue;
                    graphView.AddUngroupedNode(this);
                    
                }
                else
                {
                    Group currentGroup = Group;
                    graphView.RemoveGroupedNode(this, Group);
                    DialogueName = callback.newValue;
                    graphView.AddGroupedNode(this, currentGroup);
                }
            });
            dialogueNameTextField.AddClasses("node_textfield", "node_filename-textfield", "node_textfield_hidden");

            titleContainer.Insert(0, dialogueNameTextField);
            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input,
                Port.Capacity.Multi);
            inputPort.portName = "Dialogue Connection";
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("node_custom-data-container");
            Foldout textFouldout = ElementUtility.CreateFoldout("DialogueText");
            TextField textField =  ElementUtility.CreateTextField(Text);
            textField.AddClasses("node_textfield", "node_quote-textfield");
            textFouldout.Add(textField);
            customDataContainer.Add(textFouldout);
            extensionContainer.Add(customDataContainer);
            RefreshExpandedState();
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }
}