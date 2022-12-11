using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class ExpSkill : PassiveSkill
    {
        [SerializeField] private float expMul = 1.2f;
        private Unit owner;
      

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public ExpSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown,int tier,string[] upgradeDescr, float expMul) : base(Name, description, icon, animationObject, cooldown,tier, upgradeDescr)
        {
            this.expMul = expMul;
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Exp multiplier: ", expMul.ToString()));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            unit.ExperienceManager.ExpGained += ReactToExpGain;
        }
        public override void UnbindSkill(Unit unit)
        {
            owner.ExperienceManager.ExpMultiplier = 1;
            unit.ExperienceManager.ExpGained -= ReactToExpGain;
            this.owner = null;
        }
        private void ReactToExpGain(int expGained, int exp)
        {
            owner.ExperienceManager.ExpMultiplier = expMul;
        }
      
    }
}