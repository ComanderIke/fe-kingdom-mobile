using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using Game.GameActors.Items;
using Game.GameActors.Units;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class LGFightEventDialogSO :LGEventDialogSO
    {
        
        [field:SerializeField] public UnitBP Enemy { get; set; }
   
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events, UnitBP enemy)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog, resources, rewardItems, events);
            Enemy = enemy;
        }
    }
}