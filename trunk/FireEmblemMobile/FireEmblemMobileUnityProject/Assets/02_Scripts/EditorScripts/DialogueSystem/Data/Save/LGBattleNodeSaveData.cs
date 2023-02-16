using System;
using Game.GameActors.Units;
using UnityEngine;

[Serializable]
public class LGBattleNodeSaveData :LGEventNodeSaveData
{
   
    [field:SerializeField] public EnemyArmyData EnemyArmy { get; set; }
    
}