using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class SingleChoiceNode : DialogNode
    {
        public override void Initialize(LGGraphView graphView,Vector2 position)
        {
            base.Initialize(graphView,position);
            DialogType = DialogType.SingleChoice;
            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}