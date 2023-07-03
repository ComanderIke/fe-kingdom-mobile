using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class PassiveSkill : Skill
    {
      
        public PassiveSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
        }
        
    }
}