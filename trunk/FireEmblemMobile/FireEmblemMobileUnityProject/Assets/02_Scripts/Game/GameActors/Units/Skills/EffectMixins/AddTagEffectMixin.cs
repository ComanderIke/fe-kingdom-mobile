using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Tag", fileName = "TagEffect")]
    public class AddTagEffectMixin : UnitTargetSkillEffectMixin
    {
        public UnitTags []tags;
        private bool activated = false;
        public override void Activate(Unit target, Unit caster, int level)
        {

            target.AddTag(tags[level]);
            activated = true;

        }

       

        public override void Deactivate(Unit target, Unit caster, int level)
        {
            if(activated)
                target.RemoveTag(tags[level]);
            activated = false;
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
           
         
            return new List<EffectDescription>()
            {
                new EffectDescription("Gain Tag: ", tags[level].ToString(),
                    tags[level+1].ToString())
            };
        }

      
    }
}