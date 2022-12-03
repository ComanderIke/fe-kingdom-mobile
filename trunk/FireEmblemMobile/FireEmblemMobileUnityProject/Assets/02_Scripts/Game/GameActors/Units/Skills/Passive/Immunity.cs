using System;
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


        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public Immunity(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, ImmunityType type) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.type = type;
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