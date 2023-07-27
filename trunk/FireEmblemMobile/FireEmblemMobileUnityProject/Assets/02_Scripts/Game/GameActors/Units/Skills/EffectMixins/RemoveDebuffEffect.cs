using System.Collections.Generic;
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
            return new List<EffectDescription>()
            {
                new EffectDescription("RemoveDebuff: ", "" + "TODO",
                    "" + "TODO")
            };
        }

    
    }
}