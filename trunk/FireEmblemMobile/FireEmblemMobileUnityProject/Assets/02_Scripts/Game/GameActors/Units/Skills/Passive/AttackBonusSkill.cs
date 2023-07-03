using System.Collections.Generic;
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

        public AttackBonusSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr, int attackBonus) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.attackBonus = attackBonus;
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}