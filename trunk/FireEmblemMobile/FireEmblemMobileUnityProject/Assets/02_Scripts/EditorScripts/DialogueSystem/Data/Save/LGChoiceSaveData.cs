using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
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
       [field: SerializeField] public List<ItemBP> ItemRequirements { get; set; }
       public LGChoiceSaveData()
       {
           ItemRequirements = new List<ItemBP>();
           CharacterRequirements = new List<UnitBP>();
           AttributeRequirements = new List<ResponseStatRequirement>();
       }
    }
}