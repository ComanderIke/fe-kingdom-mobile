using System.Collections.Generic;
using __2___Scripts.External.Editor.Elements;
using Game.GameActors.Items;
using Game.GameActors.Units;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class LGBattleEventDialogSO :LGEventDialogSO
    {
        
        [field:SerializeField] public EnemyArmyData EnemyArmy { get; set; }
   
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, string headline,List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events, EnemyArmyData enemyArmy)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog,headline, resources, rewardItems, events);
            EnemyArmy = enemyArmy;
        }
    }
}