using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameMechanics;
using UnityEngine;

namespace Game.Dialog.DialogSystem
{
    [Serializable]
    public class LGDialogChoiceData
    {
        [field:SerializeField]public string Text { get; set; }
        [field:SerializeField]public LGDialogSO NextDialogue { get; set; }
        [field:SerializeField]public LGDialogSO NextDialogueFail { get; set; }
        [field:SerializeField]public List<LGDialogSO> NextDialogueRandom { get; set; }
        [field: SerializeField] public List<ResponseStatRequirement> AttributeRequirements { get; set; }
        [field: SerializeField] public List<UnitBP> CharacterRequirements { get; set; }
        [field: SerializeField] public List<ItemBP> ItemRequirements { get; set; }
        [field: SerializeField] public List<BlessingBP> BlessingRequirements { get; set; }
        [field: SerializeField] public List<ResourceEntry> ResourceRequirements { get; set; }
        [field: SerializeField] public int RandomRate;


    }
}