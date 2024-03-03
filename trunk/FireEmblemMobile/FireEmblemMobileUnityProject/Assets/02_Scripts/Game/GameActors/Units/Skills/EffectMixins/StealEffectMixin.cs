using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Steal", fileName = "StealEffect")]
    public class StealEffectMixin : UnitTargetSkillEffectMixin
    {
        public int []stealAmountMin;
        public int []stealAmountMax;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;
 

        public override void Activate(Unit target, Unit caster, int level)
        {

            int min = stealAmountMin[level];
            int max = stealAmountMax[level];
            int stealAmount = Random.Range(min, max + 1);
            int scalingAttribute = (int)(target.Stats.CombinedAttributes().GetAttributeStat(scalingType) *
                                  scalingcoeefficient[level]);
            target.Party.Money += stealAmount+scalingAttribute;


        }

       

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            string upgLabel = "";
            string valueLabel = "";
            if (level < scalingcoeefficient.Length)
            {
                valueLabel += scalingcoeefficient[level];
            }
            if (level+1 < scalingcoeefficient.Length)
            {
                upgLabel += scalingcoeefficient[level + 1];
            }
            else
            {
                upgLabel = valueLabel;
            }

            return new List<EffectDescription>()
            {
                new EffectDescription("Stealamount: ", "" + stealAmountMin[level]+"-"+stealAmountMax[level],
                    "" + stealAmountMin[level + 1]+"-"+stealAmountMax[level+1]),
                new EffectDescription("Scaling "+scalingType+": ", valueLabel, upgLabel)
            };
        }

      
    }

    public enum UnitTags
    {
        BombExpertLv1,
        BombExpertLv2,
        BombExpertLv3
    }
}