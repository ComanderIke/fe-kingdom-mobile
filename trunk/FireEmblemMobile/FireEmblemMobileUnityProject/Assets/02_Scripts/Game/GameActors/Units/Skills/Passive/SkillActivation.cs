using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class SkillActivation : PassiveSkill
    {

        private Unit owner;
        [SerializeField] float procChance;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public SkillActivation(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
            this.procChance = procChance;
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Activation rate: ", procChance.ToString()));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            unit.BonusSkillProcChance += procChance;
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.BonusSkillProcChance -= procChance;
            this.owner = null;
        }
      
    }
}