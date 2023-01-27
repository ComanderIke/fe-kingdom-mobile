using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class MultiChoiceNode : DialogNode
    {
        public override void Initialize(LGGraphView graphView,Vector2 position)
        {
            base.Initialize(graphView,position);
            DialogType = DialogType.MultiChoice;
            
            Choices.Add("Next Dialogue");
            
        }

        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = ElementUtility.CreateButton("Add Choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");
                Choices.Add("new Choice");
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("node_button");
            mainContainer.Insert(1, addChoiceButton);
            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        Port CreateChoicePort(string choice)
        {
            Port choicePort =this.CreatePort();
            choicePort.portName = "";
            Button deleteChoiceButton = ElementUtility.CreateButton("X");
            deleteChoiceButton.AddToClassList("node_button");
            TextField choiceTextField =  ElementUtility.CreateTextField(choice);
            choiceTextField.AddClasses("node_textfield", "node_choice-textfield", "node_textfield_hidden");
           
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
    }
}