using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
   
    public class EventNode : DialogNode
    {
        
        public List<ItemBP> ItemRewards;
        public List<ResourceEntry> ResourceRewards;
        private Dictionary<ResourceEntry, TextField> resourceTextFields;
        public List<DialogEvent> Events;
        private VisualElement resourceContainer;
        private VisualElement resourcesContainer;
        
        public override void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            
            base.Initialize(nodeName, graphView, position);
            ItemRewards = new List<ItemBP>();
            Events = new List<DialogEvent>();
            Events.Add(ScriptableObject.CreateInstance<NullDialogEvent>());
            ItemRewards.Add(ScriptableObject.CreateInstance<ItemBP>());
            ResourceRewards = new List<ResourceEntry>();
            resourceTextFields = new Dictionary<ResourceEntry, TextField>();
            
        }

        public override void Draw()
        {
            base.Draw();
            Foldout rewardFouldout = ElementUtility.CreateFoldout("Rewards", true);
            resourcesContainer = new VisualElement();
            foreach (ResourceEntry reward in ResourceRewards)
            {

                resourceTextFields[reward]=ElementUtility.CreateTextIntFieldAndLabel(reward.Amount.ToString(),reward.ResourceType.ToString(),callback=>
                {
                    resourceTextFields[reward].value = AllowOnlyNumbers(callback.newValue);
                });
                var popUp=ElementUtility.CreatePopup<ResourceType>(Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList(),ResourceType.Gold);
                resourcesContainer.Add(resourceTextFields[reward]);
                resourcesContainer.Add(popUp);
            }
            

            VisualElement buttonContainer = new VisualElement();
            buttonContainer.AddToClassList("lg-horizontal");
            rewardFouldout.Add(buttonContainer);
            Button addResourceButton = ElementUtility.CreateButton("Add Resource", () =>
            {
                var entry=new ResourceEntry(0, ResourceType.Gold);
                resourceContainer = new VisualElement();
                resourceContainer.AddToClassList("lg-horizontal");
                resourceTextFields.Add(entry,new TextField());

                resourceTextFields[entry]=ElementUtility.CreateTextIntField(entry.Amount.ToString(),callback=>
                {
                    resourceTextFields[entry].value = AllowOnlyNumbers(callback.newValue);
                });
                var popUp=ElementUtility.CreatePopup<ResourceType>(Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList(),ResourceType.Gold);
                resourceContainer.Add(resourceTextFields[entry]);
                resourceContainer.Add(popUp);
                resourcesContainer.Add(resourceContainer);

                ResourceRewards.Add(entry);
              
            });
            addResourceButton.AddToClassList("node_button");
            buttonContainer.Add(addResourceButton);
            Button removeResourceButton = ElementUtility.CreateButton("Remove Resource", () =>
            {
                resourcesContainer.RemoveAt(resourcesContainer.childCount-1);
                ResourceRewards.RemoveAt(ResourceRewards.Count-1);
              
            });
            removeResourceButton.AddToClassList("node_button");
            buttonContainer.Add(removeResourceButton);
            rewardFouldout.Add(resourcesContainer);
            VisualElement itemButtonContainer = new VisualElement();
            itemButtonContainer.AddToClassList("lg-horizontal");
            Button addChoiceButton = ElementUtility.CreateButton("Add Item", () =>
            {
                ObjectField itemReward = new ObjectField("Item");
                
                itemReward.objectType = typeof(ItemBP);
                ItemRewards.Add(ScriptableObject.CreateInstance<ItemBP>());
                rewardFouldout.Add(itemReward);
            });
            addChoiceButton.AddToClassList("node_button");
            itemButtonContainer.Add(addChoiceButton);
            Button removeItemButton = ElementUtility.CreateButton("Remove Item", () =>
            {
                ItemRewards.RemoveAt(ItemRewards.Count-1);
                rewardFouldout.RemoveAt(rewardFouldout.childCount-1);
            });
            removeItemButton.AddToClassList("node_button");
            itemButtonContainer.Add(removeItemButton);
            rewardFouldout.Add(itemButtonContainer);
            foreach (var itemReward in ItemRewards)
            {
                ObjectField itemRewardField = new ObjectField("Item");
                itemRewardField.objectType = typeof(ItemBP);
                rewardFouldout.Add(itemRewardField);
            }
            
           
            Foldout eventFouldout = ElementUtility.CreateFoldout("Events", true);
            VisualElement eventContainer = new VisualElement();
            eventContainer.AddToClassList("lg-horizontal");
            Button addEventButton = ElementUtility.CreateButton("Add Event", () =>
            {
                ObjectField eventField = new ObjectField("Event");
                eventField.objectType = typeof(DialogEvent);
                eventFouldout.Add(eventField);
                Events.Add(ScriptableObject.CreateInstance<NullDialogEvent>());
            });
            addEventButton.AddToClassList("node_button");
            eventContainer.Add( addEventButton);
            Button removeEventButton = ElementUtility.CreateButton("Remove Event", () =>
            {
                Events.RemoveAt(Events.Count-1);
                eventFouldout.RemoveAt(eventFouldout.childCount-1);
            });
            removeEventButton.AddToClassList("node_button");
            eventContainer.Add(removeEventButton);
            eventFouldout.Add(eventContainer);
            foreach (var dialogEvent in Events)
            {
                ObjectField eventField = new ObjectField("Event");
                eventField.objectType = typeof(DialogEvent);
                eventFouldout.Add(eventField);
            }
            RefreshExpandedState();
            customDataContainer.Add(rewardFouldout);
            customDataContainer.Add(eventFouldout);
        }
        string AllowOnlyNumbers( string newValue)
        {
            return Regex.Replace(newValue, @"[^0-9]", "");
            
           
        }
       
    }
}