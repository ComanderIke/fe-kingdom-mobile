using System;
using System.Collections.Generic;
using System.Linq;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class DialogNode:Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<LGChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public bool PortraitLeft { get; set; }
        public DialogActor DialogActor { get; set; }
        public DialogGroup Group { get; set; }
        public DialogType DialogType { get; set; }
        private Color defaultBackgroundColor;
        protected LGGraphView graphView;
        protected VisualElement customDataContainer;

        public virtual void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
           
            DialogueName = nodeName;
            Choices = new List<LGChoiceSaveData>();
            Text = "DialogueText";
            PortraitLeft = true;
            this.graphView = graphView;
            defaultBackgroundColor = new Color(29f/255f, 29/255f, 30/255f);
            SetPosition(new Rect(position, Vector2.zero));
            mainContainer.AddToClassList("node_main-container");
            extensionContainer.AddToClassList("node_extension-container");
        }

        public virtual void Draw()
        {
            TextField dialogueNameTextField = ElementUtility.CreateTextField(DialogueName,null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty((target.value)))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = target.value;
                    graphView.AddUngroupedNode(this);
                    
                }
                else
                {
                    DialogGroup currentGroup = Group;
                    graphView.RemoveGroupedNode(this, Group);
                    DialogueName =  target.value;
                    graphView.AddGroupedNode(this, currentGroup);
                }
            });
            dialogueNameTextField.AddClasses("node_textfield", "node_filename-textfield", "node_textfield_hidden");

            titleContainer.Insert(0, dialogueNameTextField);
            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input,
                Port.Capacity.Multi);
            //inputPort.portName = "Dialogue Connection";
            inputContainer.Add(inputPort);

            customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("node_custom-data-container");
            Foldout textFouldout = ElementUtility.CreateFoldout("DialogueText", true);
            Toggle toggle = ElementUtility.CreateToggle("PortraitIsLeft", true);
            toggle.value = PortraitLeft;
            toggle.RegisterValueChangedCallback(callback =>
            {
                PortraitLeft = callback.newValue;
            });
            TextField textField =  ElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });
            textField.AddClasses("node_textfield", "node_quote-textfield");
            
            textFouldout.Add(textField);
            ObjectField prop = new ObjectField("Actor");
            prop.objectType = typeof(DialogActor);
            prop.value = DialogActor;
            prop.RegisterValueChangedCallback(callback =>
            {
                DialogActor = (DialogActor)callback.newValue;
            });
            customDataContainer.Add(prop);
            customDataContainer.Add(toggle);
            customDataContainer.Add(textFouldout);
            
            //extensionContainer.Add(prop);
            extensionContainer.Add(customDataContainer);
            RefreshExpandedState();
        }

       
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent=> DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent=> DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }
        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children().Where(p=> p is Port))
            {
                if (!port.connected)
                {
                    continue;
                }
                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port)inputContainer.Children().First();
            return !inputPort.connected;
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