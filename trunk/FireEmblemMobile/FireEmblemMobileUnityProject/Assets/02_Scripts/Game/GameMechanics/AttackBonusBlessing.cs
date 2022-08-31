using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class AttackBonusBlessing : Blessing, IDamageInfluencer
    {
        [SerializeField] private int attackBonus = 3;
        private Unit owner;
        public void Init(Unit unit)
        {
            this.owner = unit;
            unit.BattleComponent.BattleStats.AddDamageInfluencer(this);
        }

        public int InfluenceDamage(int damage)
        {
            return damage + attackBonus;
        }
    }

    public interface IDamageInfluencer
    {
    }
}