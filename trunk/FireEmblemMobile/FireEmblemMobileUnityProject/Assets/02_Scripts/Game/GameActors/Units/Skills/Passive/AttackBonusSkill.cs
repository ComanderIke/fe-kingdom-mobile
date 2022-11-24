using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class AttackBonusSkill : PassiveSkill, IDamageInfluencer
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

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }
    }
}