using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;

[Serializable]
public class AuraDuringCombatSkill : PassiveSkill
{
   public AuraDuringCombatSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, tier,upgradeDescr)
    {
    }

   public override List<EffectDescription> GetEffectDescription()
   {
       throw new NotImplementedException();
   }
}