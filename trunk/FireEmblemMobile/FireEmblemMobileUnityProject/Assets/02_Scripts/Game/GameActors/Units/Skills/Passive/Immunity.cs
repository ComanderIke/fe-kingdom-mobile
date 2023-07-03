using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public enum ImmunityType
    {
        Critical,
        Effective,
        PhysDamage,
        MagicDamage,
        invulnerable
    }
    [Serializable]
    public class Immunity:PassiveSkill
    {
        private ImmunityType type;

        public Immunity(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr, ImmunityType type) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.type = type;
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }

        public override void BindSkill(Unit unit)
        {
   
            unit.BattleComponent.BattleStats.Immunities.Add(type);
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.BattleComponent.BattleStats.Immunities.Remove(type);

        }
    }
}