using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public struct EffectDescription
    {
        public string label;
        public string value;

        public EffectDescription(string label, string value)
        {
            this.label = label;
            this.value = value;
        }
    }
    public class OnHealingReceivedSkill : PassiveSkill
    {
        [SerializeField] private float healMult = 1.2f;
        private Unit owner;
      

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public OnHealingReceivedSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown,int tier,string[] upgradeDescr, float healMult) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
            this.healMult = healMult;
        }

       
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Heal Multiplier: ", healMult.ToString()));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            unit.BeforeHealingReceived += ReactToBeforeHealReceived;
        }
        public override void UnbindSkill(Unit unit)
        {
            owner.HealingMultiplier = 1;
            unit.BeforeHealingReceived -= ReactToBeforeHealReceived;
            this.owner = null;
        }
        private void ReactToBeforeHealReceived(int healingReceived)
        {
            owner.HealingMultiplier = healMult;
        }
      
    }
}