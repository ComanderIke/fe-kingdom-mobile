using __2___Scripts.External.Editor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class MultiChoiceNode : DialogNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogType = DialogType.MultiChoice;
            
            Choices.Add("Next Dialogue");
            
        }

        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };
            addChoiceButton.AddToClassList("node_button");
            mainContainer.Insert(1, addChoiceButton);
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
                choicePort.portName = "";
                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };
                deleteChoiceButton.AddToClassList("node_button");
                TextField choiceTextField = new TextField()
                {
                    value = choice
                };
                choiceTextField.AddToClassList("node_textfield");
                choiceTextField.AddToClassList("node_choice-textfield");
                choiceTextField.AddToClassList("node_textfield_hidden");
                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}