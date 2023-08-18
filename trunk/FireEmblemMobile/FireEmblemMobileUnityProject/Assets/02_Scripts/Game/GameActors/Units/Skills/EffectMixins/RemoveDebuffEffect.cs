﻿using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/RemoveDebuff", fileName = "RemoveDebuffEffect")]
    public class RemoveDebuffEffect : UnitTargetSkillEffectMixin
    {
       
        public List<DebuffType> removeDebuffTypes;


        public override void Activate(Unit target, Unit caster, int level)
        {
            foreach (var debuff in removeDebuffTypes)
            {
                target.StatusEffectManager.RemoveDebuff(debuff);
            }
           
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            string valueLabel = "";
            foreach (var debuff in removeDebuffTypes)
            {
                valueLabel += debuff.ToString() + "/";
            }

            valueLabel = valueLabel.Remove(valueLabel.Length - 1, 1);
            string upgLabel = valueLabel;
            return new List<EffectDescription>()
            {
                new EffectDescription("Remove: ", valueLabel,
                    upgLabel)
            };
        }

    
    }
}