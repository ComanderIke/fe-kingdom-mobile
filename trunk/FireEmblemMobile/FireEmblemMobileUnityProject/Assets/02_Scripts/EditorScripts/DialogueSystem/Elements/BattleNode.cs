using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using Game.GameActors.Units;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class BattleNode : EventNode
    {
        public EnemyArmyData EnemyArmy { get; set; }
        public override void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            base.Initialize(nodeName, graphView,position);
            DialogType = DialogType.Battle;

            LGChoiceSaveData winData = new LGChoiceSaveData()
            {
                Text = "Won"
            };
            LGChoiceSaveData lostData = new LGChoiceSaveData()
            {
                Text = "Lost"
            };
            Choices.Add(winData);
            Choices.Add(lostData);
            
        }

        public override void Draw()
        {
            base.Draw();
            ObjectField prop = new ObjectField("EnemyArmy");
            prop.objectType = typeof(EnemyArmyData);
            prop.value = EnemyArmy;
            prop.RegisterValueChangedCallback(callback =>
            {
                EnemyArmy = (EnemyArmyData)callback.newValue;
            });
            customDataContainer.Insert(1, prop);
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
            TextField choiceTextField =  ElementUtility.CreateTextField(choiceSaveData.Text, null, callback =>
            {
                choiceSaveData.Text = callback.newValue;
            });
            choiceTextField.AddClasses("node_textfield", "node_choice-textfield", "node_textfield_hidden");
           
            choicePort.Add(choiceTextField);
            return choicePort;
        }
    }
}