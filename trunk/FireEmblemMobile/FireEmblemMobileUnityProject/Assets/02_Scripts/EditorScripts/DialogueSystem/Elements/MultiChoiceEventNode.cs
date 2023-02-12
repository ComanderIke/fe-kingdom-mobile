using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class MultiChoiceEventNode : EventNode
    {
        public override void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            base.Initialize(nodeName, graphView,position);
            DialogType = DialogType.MultiChoiceEvent;

            LGChoiceSaveData choiceData = new LGChoiceSaveData()
            {
                Text = "Next Dialogue"
            };
            Choices.Add(choiceData);
            
        }

        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = ElementUtility.CreateButton("Add Choice", () =>
            {
               
                LGChoiceSaveData choiceData = new LGChoiceSaveData()
                {
                    Text = "Next Dialogue"
                };
                Choices.Add(choiceData);
                Port choicePort = CreateChoicePort(choiceData);
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("node_button");
            mainContainer.Insert(1, addChoiceButton);
            foreach (LGChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        Port CreateChoicePort(object userData )
        {
            Port choicePort =this.CreatePort();
            choicePort.userData = userData;
            LGChoiceSaveData choiceSaveData = (LGChoiceSaveData)userData;
            Button deleteChoiceButton = ElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceSaveData);
                graphView.RemoveElement(choicePort);
            });
            deleteChoiceButton.AddToClassList("node_button");
            TextField choiceTextField =  ElementUtility.CreateTextField(choiceSaveData.Text, null, callback =>
            {
                choiceSaveData.Text = callback.newValue;
            });
            choiceTextField.AddClasses("node_textfield", "node_choice-textfield", "node_textfield_hidden");
           
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
    }
}