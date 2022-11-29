using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class OnInitiateCombatSkill:PassiveSkill
    {
        public OnInitiateCombatSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
        }
    }
}