using System.Collections.Generic;
using Game.GameActors.Items;

namespace Game.Dialog.DialogSystem
{
    public class LGRandomOutcomeEventDialogSO :LGEventDialogSO
    {
        
      
   
        public void Initialize(string dialogName, DialogActor dialogActor, string text, List<LGDialogChoiceData> choices, DialogType dialogType,
            bool portraitLeft, bool isStartingDialog, string headline,List<ResourceEntry> resources, List<ItemBP> rewardItems, List<DialogEvent> events)
        {
            base.Initialize(dialogName, dialogActor, text, choices, dialogType, portraitLeft, isStartingDialog,headline, resources, rewardItems, events);
        }
    }
}