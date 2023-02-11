using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class SingleChoiceNode : DialogNode
    {
        public override void Initialize(string nodeName,LGGraphView graphView,Vector2 position)
        {
            base.Initialize(nodeName, graphView,position);
            DialogType = DialogType.SingleChoice;
            LGChoiceSaveData choiceData = new LGChoiceSaveData()
            {
                Text = "Next Dialogue"
            };
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();
            foreach (LGChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);
                choicePort.userData = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}