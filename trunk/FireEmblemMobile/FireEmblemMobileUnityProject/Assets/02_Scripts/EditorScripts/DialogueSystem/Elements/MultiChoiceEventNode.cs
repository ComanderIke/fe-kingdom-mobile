using System;
using System.Collections.Generic;
using System.Linq;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;
using ObjectField = UnityEditor.UIElements.ObjectField;

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
                AddResponse(choiceData);
              
            });
            addChoiceButton.AddToClassList("node_button");
            mainContainer.Insert(1, addChoiceButton);
            foreach (LGChoiceSaveData choice in Choices)
            {
                AddResponse(choice);
            }
            RefreshExpandedState();
        }

        void RemoveCharacterRequirement(UnitBP removeChar,LGChoiceSaveData choiceData)
        {
            choiceData.CharacterRequirement=null;
        }
        void RemoveItemRequirement(ItemBP removeItem,LGChoiceSaveData choiceData)
        {
            if( choiceData.ItemRequirements.Contains(removeItem))
                choiceData.ItemRequirements.Remove(removeItem);
        }
        void AddResponse(LGChoiceSaveData choiceData)
        {
            
            var responseContainer = new VisualElement();
            responseContainer.AddToClassList("lg-horizontal");
            Button addCharAndItemRequirement = ElementUtility.CreateButton("[C/I]", () =>
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(UnitBP);
                responseContainer.Insert(2,charItemField);
                UnitBP newUnit = ScriptableObject.CreateInstance<UnitBP>();
                ItemBP newItem = ScriptableObject.CreateInstance<ItemBP>();
                choiceData.CharacterRequirement=newUnit;
                charItemField.RegisterValueChangedCallback(callback =>
                {
                    RemoveCharacterRequirement(newUnit,choiceData);
                    RemoveItemRequirement(newItem,choiceData);
                    if (charItemField.objectType == typeof(UnitBP))
                    {
                        newUnit = (UnitBP)callback.newValue;
                        choiceData.CharacterRequirement=newUnit;
                    }
                    else if (charItemField.objectType == typeof(ItemBP))
                    {
                        newItem = (ItemBP)callback.newValue;
                        choiceData.ItemRequirements.Add(newItem);
                    }
                });
                var charItemPopup = ElementUtility.CreatePopup(new List<string> {"Character", "Item"}, "Character", callback =>
                {
                    switch (callback.newValue)
                    {
                        case "Character": charItemField.objectType = typeof(UnitBP);
                            RemoveCharacterRequirement(newUnit, choiceData);
                            RemoveItemRequirement(newItem, choiceData);
                            choiceData.CharacterRequirement=newUnit;
                            charItemField.value = null; break;
                        case "Item": charItemField.objectType = typeof(ItemBP);
                            RemoveCharacterRequirement(newUnit, choiceData);
                            RemoveItemRequirement(newItem, choiceData);
                            choiceData.ItemRequirements.Add(newItem);
                            charItemField.value = null; break;
                    }
                });
                responseContainer.Insert(3,charItemPopup);
            });
            responseContainer.Add(addCharAndItemRequirement);
          
            responseContainer.Add(addCharAndItemRequirement);
            Button addStatRequirement = ElementUtility.CreateButton("[ATR]", () =>
            {
                TextField atrAmountTextField=null;
                ResponseStatRequirement statRequirement = new ResponseStatRequirement()
                {
                    AttributeType = AttributeType.LVL,
                    Amount = 0
                };
                choiceData.AttributeRequirements.Add(statRequirement);
                atrAmountTextField=ElementUtility.CreateTextIntField("0", callback =>
                {
                    atrAmountTextField.value = ElementUtility.AllowOnlyNumbers(callback.newValue);
                    statRequirement.Amount =  Int32.Parse(atrAmountTextField.value);
                    
                });
                responseContainer.Insert(2,atrAmountTextField);
                var atrPopup = ElementUtility.CreatePopup(Enum.GetValues(typeof(AttributeType)).Cast<AttributeType>().ToList(), AttributeType.LVL, callback =>
                {
                    statRequirement.AttributeType = callback.newValue;
                });
                responseContainer.Insert(3,atrPopup);
            });
            responseContainer.Add(addStatRequirement);
            if (choiceData.CharacterRequirement!=null)
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(UnitBP);
                charItemField.value = choiceData.CharacterRequirement;
                responseContainer.Insert(2,charItemField);

            }
            foreach (var itemReq in choiceData.ItemRequirements)
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(ItemBP);
                charItemField.value =itemReq;
                responseContainer.Insert(2,charItemField);
            }

            foreach (var atrReq in choiceData.AttributeRequirements)
            {
                TextField atrAmountTextField = null;
                atrAmountTextField=ElementUtility.CreateTextIntField(atrReq.Amount.ToString(), callback =>
                {
                    atrAmountTextField.value = ElementUtility.AllowOnlyNumbers(callback.newValue);
                    atrReq.Amount =Int32.Parse(atrAmountTextField.value);
                });
                responseContainer.Insert(2,atrAmountTextField);
                var atrPopup = ElementUtility.CreatePopup(Enum.GetValues(typeof(AttributeType)).Cast<AttributeType>().ToList(), atrReq.AttributeType, callback =>
                {
                    atrReq.AttributeType = callback.newValue;
                });
                responseContainer.Insert(3,atrPopup);
            }

            
            Port choicePort = CreateChoicePort(choiceData);
            responseContainer.Add(choicePort);
            if ((choiceData.AttributeRequirements != null && choiceData.AttributeRequirements.Count > 0) ||
                (choiceData.CharacterRequirement != null) ||
                choiceData.ItemRequirements != null && choiceData.ItemRequirements.Count > 0)
            {
                Port failChoicePort = CreateUnDeletableChoicePort(choiceData, "Fail");
                responseContainer.Add(failChoicePort);
            }

            outputContainer.Add(responseContainer);
        }
        Port CreateUnDeletableChoicePort(object userData, string portName)
        {
            Port choicePort =this.CreatePort();
            choicePort.userData = userData;
            choicePort.portName = portName;
            //LGChoiceSaveData choiceSaveData = (LGChoiceSaveData)userData;
            choicePort.AddClasses("node_textfield", "node_choice-textfield", "node_textfield_hidden");
            // TextField choiceTextField =  ElementUtility.CreateTextField(label, null, callback =>
            // {
            //     choiceSaveData.Text = callback.newValue;
            // });
            // choiceTextField.AddClasses("node_textfield", "node_choice-textfield", "node_textfield_hidden");
            // choicePort.Add(choiceTextField);
            return choicePort;
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