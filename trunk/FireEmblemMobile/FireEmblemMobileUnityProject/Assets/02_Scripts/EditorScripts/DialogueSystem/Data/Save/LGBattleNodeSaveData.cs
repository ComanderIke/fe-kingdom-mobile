using System;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Units;
using UnityEngine;

[Serializable]
public class LGBattleNodeSaveData :LGEventNodeSaveData
{
   
    [field:SerializeField] public BattleMap BattleMap { get; set; }
    
}