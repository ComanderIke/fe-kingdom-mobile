using System.Collections.Generic;
using System.Text.RegularExpressions;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Utility;
using Game.GameActors.Items;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.EditorScripts.DialogueSystem.Elements
{
    public class EventNode : DialogNode
    {
        private TextField goldTextField;
        private TextField expTextField;
        private TextField graceTextField;
        private List<ItemBP> itemRewards;
        private List<DialogEvent> events;
        
        public override void Initialize(string nodeName, LGGraphView graphView,Vector2 position)
        {
            
            base.Initialize(nodeName, graphView, position);
            itemRewards = new List<ItemBP>();
            events = new List<DialogEvent>();
            events.Add(ScriptableObject.CreateInstance<NullDialogEvent>());
            itemRewards.Add(ScriptableObject.CreateInstance<ItemBP>());
        }

        public override void Draw()
        {
            base.Draw();
            Foldout rewardFouldout = ElementUtility.CreateFoldout("Rewards", true);
            goldTextField=ElementUtility.CreateTextIntFieldAndLabel("0","Gold", GoldValueChangedCallback);
            rewardFouldout.Add(goldTextField);
            expTextField=ElementUtility.CreateTextIntFieldAndLabel("0","Exp", ExpValueChangedCallback);
            rewardFouldout.Add(expTextField);
            graceTextField=ElementUtility.CreateTextIntFieldAndLabel("0","Grace", GraceValueChangedCallback);
            rewardFouldout.Add(graceTextField);
            Button addChoiceButton = ElementUtility.CreateButton("Add Item", () =>
            {
                ObjectField itemReward = new ObjectField("Item");
                
                itemReward.objectType = typeof(ItemBP);
                itemRewards.Add(ScriptableObject.CreateInstance<ItemBP>());
                rewardFouldout.Add(itemReward);
            });
            addChoiceButton.AddToClassList("node_button");
            rewardFouldout.Add(addChoiceButton);
            Button removeItemButton = ElementUtility.CreateButton("Remove Item", () =>
            {
                itemRewards.RemoveAt(itemRewards.Count-1);
                rewardFouldout.RemoveAt(rewardFouldout.childCount-1);
            });
            removeItemButton.AddToClassList("node_button");
            rewardFouldout.Add(removeItemButton);
            foreach (var itemReward in itemRewards)
            {
                ObjectField itemRewardField = new ObjectField("Item");
                itemRewardField.objectType = typeof(ItemBP);
                rewardFouldout.Add(itemRewardField);
            }
            
           
            Foldout eventFouldout = ElementUtility.CreateFoldout("Events", true);
            Button addEventButton = ElementUtility.CreateButton("Add Event", () =>
            {
                ObjectField eventField = new ObjectField("Event");
                eventField.objectType = typeof(DialogEvent);
                eventFouldout.Add(eventField);
                events.Add(ScriptableObject.CreateInstance<NullDialogEvent>());
            });
            addEventButton.AddToClassList("node_button");
            eventFouldout.Add( addEventButton);
            Button removeEventButton = ElementUtility.CreateButton("Remove Event", () =>
            {
                events.RemoveAt(events.Count-1);
                eventFouldout.RemoveAt(eventFouldout.childCount-1);
            });
            removeEventButton.AddToClassList("node_button");
            eventFouldout.Add(removeEventButton);
            foreach (var dialogEvent in events)
            {
                ObjectField eventField = new ObjectField("Event");
                eventField.objectType = typeof(DialogEvent);
                eventFouldout.Add(eventField);
            }
            RefreshExpandedState();
            customDataContainer.Add(rewardFouldout);
            customDataContainer.Add(eventFouldout);
        }
        void GoldValueChangedCallback(ChangeEvent<string> callback)
        {
            string text = Regex.Replace(callback.newValue, @"[^0-9]", "");
            Text = text;
            goldTextField.value = text;
        }
        void ExpValueChangedCallback(ChangeEvent<string> callback)
        {
            string text = Regex.Replace(callback.newValue, @"[^0-9]", "");
            Text = text;
            expTextField.value = text;
        }
        void GraceValueChangedCallback(ChangeEvent<string> callback)
        {
            string text = Regex.Replace(callback.newValue, @"[^0-9]", "");
            Text = text;
            graceTextField.value = text;
        }
    }
}