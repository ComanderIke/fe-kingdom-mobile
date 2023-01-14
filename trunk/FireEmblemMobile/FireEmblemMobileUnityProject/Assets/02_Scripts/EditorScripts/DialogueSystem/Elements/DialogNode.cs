using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
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
        public DialogType DialogType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "DialogueText";
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddToClassList("node_main-container");
            extensionContainer.AddToClassList("node_extension-container");
        }

        public virtual void Draw()
        {
            TextField dialogueNameTextField = new TextField()
            {
                value = DialogueName
            };
            dialogueNameTextField.AddToClassList("node_textfield");
            dialogueNameTextField.AddToClassList("node_filename-textfield");
            dialogueNameTextField.AddToClassList("node_textfield_hidden");
           
            titleContainer.Insert(0, dialogueNameTextField);
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Dialogue Connection";
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("node_custom-data-container");
            Foldout textFouldout = new Foldout()
            {
                text="Dialogue Text"
            };
            TextField textField = new TextField()
            {
                value = Text
            };
            textField.AddToClassList("node_textfield");
            textField.AddToClassList("node_quote-textfield");
            textFouldout.Add(textField);
            customDataContainer.Add(textFouldout);
            extensionContainer.Add(customDataContainer);
            RefreshExpandedState();
        }
    }
}