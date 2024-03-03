using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGEventDialogSO :LGDialogSO
    {
        [field:SerializeField] public string HeadLine { get; set; }
        [field:SerializeField] public List<ResourceEntry> RewardResources { get; set; }
        [field:SerializeField] public List<ItemBP> RewardItems { get; set; }
        [field:SerializeField] public List<DialogEvent> Events { get; set; }
        

        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, string headline,List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog);
            RewardResources = resources;
            RewardItems = rewardItems;
            Events = events;
            HeadLine = headline;
        }
    }
}