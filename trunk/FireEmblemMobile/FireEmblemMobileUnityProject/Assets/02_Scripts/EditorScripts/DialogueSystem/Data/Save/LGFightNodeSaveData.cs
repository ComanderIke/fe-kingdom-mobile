using System;
using Game.GameActors.Units;
using UnityEngine;

[Serializable]
public class LGFightNodeSaveData :LGEventNodeSaveData
{
    [field:SerializeField] public UnitBP Enemy { get; set; }
    
}