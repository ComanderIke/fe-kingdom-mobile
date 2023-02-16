using System;
using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using UnityEngine;

[Serializable]
public class LGEventNodeSaveData :LGNodeSaveData
{
    [field:SerializeField]  public string Headline{ get; set; }
    [field:SerializeField] public List<ResourceEntry> RewardResources { get; set; }
    [field:SerializeField] public List<ItemBP> RewardItems { get; set; }
    [field:SerializeField] public List<DialogEvent> Events { get; set; }
 
}