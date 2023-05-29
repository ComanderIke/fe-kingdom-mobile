using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.WorldMapStuff.Model;
using GameEngine;

namespace Game.Mechanics
{
    public class UnitBattleStats
    {
        public int DamageDealt { get; set; }
    }
    public class BattleStatsSystem:IEngineSystem
    {
        private List<Unit> defeatedEnemies;
        private List<Unit> defeatedEliteEnemies;
        private Dictionary<Unit, UnitBattleStats> unitBattleStats;
        public void Init()
        {
            unitBattleStats = new Dictionary<Unit, UnitBattleStats>();
       
        }

        void UnitDealingDamage(Unit unit, int damage)
        {
            if (!unit.Faction.IsPlayerControlled)
                return;
            if(!unitBattleStats.ContainsKey(unit))
                unitBattleStats.Add(unit, new UnitBattleStats());
            unitBattleStats[unit].DamageDealt+=damage;
        }
        void UnitDefeated(Unit unit)
        {
            if (unit.Faction.Id != FactionId.ENEMY)
                return;
            if(!unit.ClassUpgraded)
                defeatedEnemies.Add(unit);
            else
            {
                defeatedEliteEnemies.Add(unit);
            }
        }
        public void Deactivate()
        {
            Unit.UnitDied-=UnitDefeated;
            Unit.OnDealingDamage -= UnitDealingDamage;
        }

        public void Activate()
        {

            Unit.UnitDied+=UnitDefeated;
            Unit.OnDealingDamage += UnitDealingDamage;
        }

        public List<Unit> GetDefeatedEnemies()
        {
            return defeatedEnemies;
        }

        public List<Unit> GetDefeatedEliteEnemies()
        {
            return defeatedEliteEnemies;
        }

        public int GetDefeatedEnemyCount()
        {
            return defeatedEnemies.Count;
        }

        public int GetDefeatedEliteEnemyCount()
        {
            return defeatedEliteEnemies.Count;
        }

        public Unit GetMvp()
        {
            throw new System.NotImplementedException();
        }
    }
}