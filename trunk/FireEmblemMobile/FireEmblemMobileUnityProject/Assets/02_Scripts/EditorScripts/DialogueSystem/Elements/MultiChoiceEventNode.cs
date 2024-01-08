using System;
using System.Collections.Generic;
using System.Linq;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.External.Editor.Elements;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using LostGrace;
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
            if( choiceData.CharacterRequirements.Contains(removeChar))
                choiceData.CharacterRequirements.Remove(removeChar);
        }
        void RemoveItemRequirement(ItemBP removeItem,LGChoiceSaveData choiceData)
        {
            if( choiceData.ItemRequirements.Contains(removeItem))
                choiceData.ItemRequirements.Remove(removeItem);
        }
        void RemoveBlessingRequirement(BlessingBP removeBlessing,LGChoiceSaveData choiceData)
        {
            if( choiceData.BlessingRequirements.Contains(removeBlessing))
                choiceData.BlessingRequirements.Remove(removeBlessing);
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
                BlessingBP newBlessing = ScriptableObject.CreateInstance<BlessingBP>();
                charItemField.RegisterValueChangedCallback(callback =>
                {
                    RemoveCharacterRequirement(newUnit,choiceData);
                    RemoveItemRequirement(newItem,choiceData);
                    RemoveBlessingRequirement(newBlessing,choiceData);
                    if (charItemField.objectType == typeof(UnitBP))
                    {
                        newUnit = (UnitBP)callback.newValue;
                        choiceData.CharacterRequirements.Add(newUnit);
                    }
                    else if (charItemField.objectType == typeof(ItemBP))
                    {
                        newItem = (ItemBP)callback.newValue;
                        choiceData.ItemRequirements.Add(newItem);
                    }
                    else if (charItemField.objectType == typeof(BlessingBP))
                    {
                        newBlessing = (BlessingBP)callback.newValue;
                        choiceData.BlessingRequirements.Add(newBlessing);
                    }
                   
                });
                var charItemPopup = ElementUtility.CreatePopup(new List<string> {"Character", "Item", "Blessing"}, "Character", callback =>
                {
                    switch (callback.newValue)
                    {
                        case "Character": charItemField.objectType = typeof(UnitBP);
                            RemoveCharacterRequirement(newUnit, choiceData);
                            RemoveItemRequirement(newItem, choiceData);
                            RemoveBlessingRequirement(newBlessing, choiceData);
                        
                            choiceData.CharacterRequirements.Add(newUnit);
                            charItemField.value = null; break;
                        case "Item": charItemField.objectType = typeof(ItemBP);
                            RemoveCharacterRequirement(newUnit, choiceData);
                            RemoveItemRequirement(newItem, choiceData);
                            RemoveBlessingRequirement(newBlessing, choiceData);
                    
                            choiceData.ItemRequirements.Add(newItem);
                            charItemField.value = null; break;
                        case "Blessing": charItemField.objectType = typeof(BlessingBP);
                            RemoveCharacterRequirement(newUnit, choiceData);
                            RemoveItemRequirement(newItem, choiceData);
                            RemoveBlessingRequirement(newBlessing, choiceData);
                    
                            choiceData.BlessingRequirements.Add(newBlessing);
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
            Button addResRequirement = ElementUtility.CreateButton("[RES]", () =>
            {
                TextField atrAmountTextField=null;
                ResourceEntry resourceEntry = new ResourceEntry(0, ResourceType.Gold);
                choiceData.ResourceRequirements.Add(resourceEntry);
                atrAmountTextField=ElementUtility.CreateTextIntField("0", callback =>
                {
                    atrAmountTextField.value = ElementUtility.AllowOnlyNumbers(callback.newValue);
                    resourceEntry.Amount =  Int32.Parse(atrAmountTextField.value);
                    
                });
                responseContainer.Insert(2,atrAmountTextField);
                var atrPopup = ElementUtility.CreatePopup(Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList(), ResourceType.Gold, callback =>
                {
                    resourceEntry.ResourceType = callback.newValue;
                });
                responseContainer.Insert(3,atrPopup);
            });
            responseContainer.Add(addResRequirement);
         
            foreach (var charReq in choiceData.CharacterRequirements)
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(UnitBP);
                charItemField.value =charReq;
                charItemField.RegisterValueChangedCallback(callback =>
                {
                    RemoveCharacterRequirement(charReq,choiceData);
                    if (charItemField.objectType == typeof(UnitBP)&&(UnitBP)callback.newValue!=null)
                    {
                        var newUnit = (UnitBP)callback.newValue;
                        choiceData.CharacterRequirements.Add(newUnit);
                    }
                   
                });
                responseContainer.Insert(2,charItemField);
            }
            foreach (var blessingReq in choiceData.BlessingRequirements)
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(BlessingBP);
                charItemField.value =blessingReq;
                charItemField.RegisterValueChangedCallback(callback =>
                {
                    RemoveBlessingRequirement(blessingReq,choiceData);
                    if (charItemField.objectType == typeof(BlessingBP)&&(BlessingBP)callback.newValue!=null)
                    {
                        var newUnit = (BlessingBP)callback.newValue;
                        choiceData.BlessingRequirements.Add(newUnit);
                    }
                   
                });
                responseContainer.Insert(2,charItemField);
            }
            foreach (var itemReq in choiceData.ItemRequirements)
            {
                ObjectField charItemField = new ObjectField();
                charItemField.objectType = typeof(ItemBP);
                charItemField.value =itemReq;
                charItemField.RegisterValueChangedCallback(callback =>
                {
                    RemoveItemRequirement(itemReq,choiceData);
                    MyDebug.LogTest("VALUECHANGEDCALLBACK ITEMREQ");
                    if (charItemField.objectType == typeof(ItemBP)&&(ItemBP)callback.newValue!=null)
                    {
                        var newUnit = (ItemBP)callback.newValue;
                        choiceData.ItemRequirements.Add(newUnit);
                    }
                   
                });
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
            foreach (var resReq in choiceData.ResourceRequirements)
            {
                TextField atrAmountTextField = null;
                atrAmountTextField=ElementUtility.CreateTextIntField(resReq.Amount.ToString(), callback =>
                {
                    atrAmountTextField.value = ElementUtility.AllowOnlyNumbers(callback.newValue);
                    resReq.Amount =Int32.Parse(atrAmountTextField.value);
                });
                responseContainer.Insert(2,atrAmountTextField);
                var atrPopup = ElementUtility.CreatePopup(Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList(), resReq.ResourceType, callback =>
                {
                    resReq.ResourceType = callback.newValue;
                });
                responseContainer.Insert(3,atrPopup);
            }

            
            Port choicePort = CreateChoicePort(choiceData);
            responseContainer.Add(choicePort);
            if ((choiceData.AttributeRequirements != null && choiceData.AttributeRequirements.Count > 0) 
                ||(choiceData.ResourceRequirements != null && choiceData.ResourceRequirements.Count > 0)||
                (choiceData.BlessingRequirements != null && choiceData.BlessingRequirements.Count > 0)||
                ( choiceData.CharacterRequirements != null && choiceData.CharacterRequirements.Count > 0) ||
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