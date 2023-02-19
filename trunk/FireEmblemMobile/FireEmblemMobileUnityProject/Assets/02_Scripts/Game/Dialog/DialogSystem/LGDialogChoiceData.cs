using System;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Data.Save;
using Game.GameActors.Items;
using Game.GameActors.Units;
using UnityEngine;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    [Serializable]
    public class LGDialogChoiceData
    {
        [field:SerializeField]public string Text { get; set; }
        [field:SerializeField]public LGDialogSO NextDialogue { get; set; }
        [field:SerializeField]public LGDialogSO NextDialogueFail { get; set; }
        [field: SerializeField] public List<ResponseStatRequirement> AttributeRequirements { get; set; }
        [field: SerializeField] public UnitBP CharacterRequirement { get; set; }
        [field: SerializeField] public List<ItemBP> ItemRequirements { get; set; }
        
    }
}