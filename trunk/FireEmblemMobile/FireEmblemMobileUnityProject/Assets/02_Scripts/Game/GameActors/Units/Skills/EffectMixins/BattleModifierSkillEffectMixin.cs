﻿using System.Collections.Generic;
using LostGrace;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/BattleModifier", fileName = "BattleModifierSkillEffect")]
    public class BattleModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public bool excessHitToCrit = false;
        public bool movementToDmg = false;
        public bool desperationEffect = false;
        public bool vantage;
        public int[] multiplier;

        public override void Activate(Unit user, int level)
        {
            Debug.Log("ACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = excessHitToCrit;
            user.BattleComponent.BattleStats.MovementToDmg = movementToDmg;
            if(movementToDmg)
                user.BattleComponent.BattleStats.MovementToDmgMultiplier = multiplier[level];
        }
        

        public override void Deactivate(Unit user, int skillLevel)
        {
            Debug.Log("DEACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = false;
            user.BattleComponent.BattleStats.MovementToDmg = false;
            user.BattleComponent.BattleStats.MovementToDmgMultiplier = 1;
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            if (level < multiplier.Length)
            {
                string upg = ""+multiplier[level];
                if (level + 1 < multiplier.Length)
                    upg = ""+multiplier[level + 1];
                list.Add(new EffectDescription("Multiplier", ""+multiplier[level], upg));
            }
            return list;
        }

      
    }
}