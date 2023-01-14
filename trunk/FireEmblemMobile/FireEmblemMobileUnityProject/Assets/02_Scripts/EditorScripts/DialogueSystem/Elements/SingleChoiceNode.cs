﻿using __2___Scripts.External.Editor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class SingleChoiceNode : DialogNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogType = DialogType.SingleChoice;
            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}