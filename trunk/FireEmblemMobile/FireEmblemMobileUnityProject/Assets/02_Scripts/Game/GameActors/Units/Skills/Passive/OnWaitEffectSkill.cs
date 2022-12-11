using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    
    public class OnWaitEffectSkill:PassiveSkill
    {
        public OnWaitEffectSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
        }
    }
}