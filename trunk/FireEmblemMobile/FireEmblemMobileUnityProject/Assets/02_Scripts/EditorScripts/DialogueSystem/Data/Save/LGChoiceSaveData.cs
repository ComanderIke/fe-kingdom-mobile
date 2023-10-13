using System;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GameActors.Units;
using LostGrace;
using UnityEngine;

namespace __2___Scripts.External.Editor.Data.Save
{
    [Serializable]
    public class LGChoiceSaveData
    {
        
        [field: SerializeField] public string Text { get; set; }
       [field: SerializeField] public string NodeID { get; set; }
       [field: SerializeField] public string NodeFailID { get; set; }
       [field: SerializeField] public List<ResponseStatRequirement> AttributeRequirements { get; set; }
       [field: SerializeField] public List<UnitBP> CharacterRequirements { get; set; }
       
       [field: SerializeField] public List<ResourceEntry> ResourceRequirements { get; set; }
       [field: SerializeField] public List<ItemBP> ItemRequirements { get; set; }
       [field: SerializeField] public List<BlessingBP> BlessingRequirements { get; set; }
     

       public LGChoiceSaveData()
       {
           ItemRequirements = new List<ItemBP>();
           ResourceRequirements = new List<ResourceEntry>();
           AttributeRequirements = new List<ResponseStatRequirement>();
           CharacterRequirements = new List<UnitBP>();
           BlessingRequirements = new List<BlessingBP>();
       }
    }
}