using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Items;
using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    public class LGBattleEventDialogSO :LGEventDialogSO
    {
        
        [field:SerializeField] public BattleMap battleMap { get; set; }
   
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, string headline,List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events,  BattleMap enemyArmy)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog,headline, resources, rewardItems, events);
            battleMap = enemyArmy;
        }
    }
}