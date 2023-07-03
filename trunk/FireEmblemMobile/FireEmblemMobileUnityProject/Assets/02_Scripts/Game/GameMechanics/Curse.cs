using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
   

    public class Curse : PassiveSkill
    {
        public Curse(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr):base(Name, description,icon, animationObject, tier, upgradeDescr)
        {
            SkillType = SkillType.Curse;
        }


        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }
    }
}