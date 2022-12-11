using System.Collections.Generic;
using System.Runtime.InteropServices;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class GrowthsSkill : PassiveSkill
    {
        [SerializeField] private Attributes growths;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public GrowthsSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier, string[] upgradeDescr, Attributes growths) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
            this.growths = growths;
        }

      
        public override void BindSkill(Unit unit)
        {
     
            unit.Stats.BonusGrowths = growths;
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.Stats.BonusGrowths.Reset();

        }
        
      
    }
}