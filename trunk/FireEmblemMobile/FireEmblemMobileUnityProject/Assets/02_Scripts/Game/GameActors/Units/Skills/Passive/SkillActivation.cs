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
        

        public SkillActivation(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.procChance = procChance;
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new System.NotImplementedException();
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