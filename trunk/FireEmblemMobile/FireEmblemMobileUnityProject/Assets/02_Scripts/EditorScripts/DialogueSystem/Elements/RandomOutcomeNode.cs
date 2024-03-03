using System;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Utility;
using Game.Dialog.DialogSystem;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class RandomOutcomeNode : EventNode
    {
       // public EnemyArmyData EnemyArmy { get; set; }
        public override void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            base.Initialize(nodeName, graphView,position);
            DialogType = DialogType.RandomOutcome;

            LGChoiceSaveData option1Data = new LGChoiceSaveData()
            {
                Text = "Option 1"
            };
            LGChoiceSaveData option2Data = new LGChoiceSaveData()
            {
                Text = "Option 2"
            };
            LGChoiceSaveData option3Data = new LGChoiceSaveData()
            {
                Text = "Option 3"
            };
            LGChoiceSaveData option4Data = new LGChoiceSaveData()
            {
                Text = "Option 4"
            };
            Choices.Add(option1Data);
            Choices.Add(option2Data);
            Choices.Add(option3Data);
            Choices.Add(option4Data);
            
        }

        public override void Draw()
        {
            base.Draw();
            // ObjectField prop = new ObjectField("EnemyArmy");
            // prop.objectType = typeof(EnemyArmyData);
            // prop.value = EnemyArmy;
            // prop.RegisterValueChangedCallback(callback =>
            // {
            //     EnemyArmy = (EnemyArmyData)callback.newValue;
            // });
            // customDataContainer.Insert(1, prop);
            foreach (LGChoiceSaveData choice in Choices)
            {
                var responseContainer = new VisualElement();
                responseContainer.AddToClassList("lg-horizontal");
                TextField atrAmountTextField=null;
               
             ;
                atrAmountTextField=ElementUtility.CreateTextIntField(""+choice.RandomRate, callback =>
                {
                    atrAmountTextField.value = ElementUtility.AllowOnlyNumbers(callback.newValue);
                    choice.RandomRate =  Int32.Parse(atrAmountTextField.value);
                    
                });
                responseContainer.Add(atrAmountTextField);
                Port choicePort = CreateChoicePort(choice);
                responseContainer.Add(choicePort);
                outputContainer.Add(responseContainer);
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