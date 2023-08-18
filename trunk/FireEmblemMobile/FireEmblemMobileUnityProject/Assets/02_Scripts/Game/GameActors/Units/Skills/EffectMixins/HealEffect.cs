using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Heal", fileName = "HealEffect")]
    public class HealEffect : UnitTargetSkillEffectMixin
    {
        public int [] heal;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;

        public override void Activate(Unit target, Unit caster, int level)
        {
            int baseDamageg = heal[level];

            int scalingdmg = (int)(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) * scalingcoeefficient[level]);
            
            target.Heal(baseDamageg+scalingdmg);
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            string valueLabel= ""+heal[level];
            if (level < heal.Length-1)
            {
                level++;
            }
            string upgLabel=""+heal[level];
            return new List<EffectDescription>()
            {
                new EffectDescription("Heal: ", valueLabel,
                    upgLabel)
            };
        }

  
    }
}