using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
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

        public AttackBonusSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, int attackBonus) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.attackBonus = this.attackBonus;
        }
    }
}