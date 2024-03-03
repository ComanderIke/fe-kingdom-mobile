using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGFightEventDialogSO :LGEventDialogSO
    {
        
        [field:SerializeField] public UnitBP Enemy { get; set; }
   
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, string headline,List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events,UnitBP enemy)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog,headline, resources, rewardItems, events);
            Enemy = enemy;
        }
    }
}