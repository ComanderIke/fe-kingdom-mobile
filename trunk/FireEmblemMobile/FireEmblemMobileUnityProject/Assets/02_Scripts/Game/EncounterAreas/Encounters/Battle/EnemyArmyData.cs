using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public abstract class EnemyArmyData : ScriptableObject
{
    
    public int level = 1;//Average Level of enemies
    public bool isBoss;

    public abstract BattleMap GetBattleMap();
}