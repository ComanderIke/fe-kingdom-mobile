using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Assasinate", fileName = "AssasinateSkillEffect")]
    public class AssasinateEffectMixin : UnitTargetSkillEffectMixin
    {
        public DamageType damageType;
        public int []hpPercentageLeft;
        public bool leaveat1hp = false;
     
        public override void Activate(Unit target, Unit caster, int level)
        {
            Debug.Log("TODO Activate Assasinate/Bane");

        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("HP %: ", "" + hpPercentageLeft[level],
                    "" + hpPercentageLeft[level + 1])
            };
        }
        
    }
}