using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using Game.GameActors.Items;
using UnityEditor;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class LGEventDialogSO :LGDialogSO
    {
        
        [field:SerializeField] public List<ResourceEntry> RewardResources { get; set; }
        [field:SerializeField] public List<ItemBP> RewardItems { get; set; }
        [field:SerializeField] public List<DialogEvent> Events { get; set; }
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog);
            RewardResources = resources;
            RewardItems = rewardItems;
            Events = events;
        }
    }
}