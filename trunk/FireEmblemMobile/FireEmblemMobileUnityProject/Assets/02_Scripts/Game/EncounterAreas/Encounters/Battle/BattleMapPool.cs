using System.Collections.Generic;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Battle
{
    [CreateAssetMenu(menuName = "GameData/BattleMapPool", fileName="BattleMapPool1")]
    public class BattleMapPool: ScriptableObject
    {
        public List<BattleMap> battleMaps;

        public BattleMap GetRandomMap()
        {
            return battleMaps[Random.Range(0, battleMaps.Count)];
        }
    }
}