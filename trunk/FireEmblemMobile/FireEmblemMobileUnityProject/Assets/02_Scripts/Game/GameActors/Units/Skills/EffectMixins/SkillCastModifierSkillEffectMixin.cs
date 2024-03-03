using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/SkillCast", fileName = "SkillCastSkillEffect")]
    public class SkillCastModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public bool freeCast;

        public override void Activate(Unit user, int level)
        {
            Debug.Log("ACTIVATE HIHIHIHIHI");
            if (freeCast)
                user.SkillManager.nextCastIsFree = true;
        }
        

        public override void Deactivate(Unit user, int skillLevel)
        {
            if (freeCast)
                user.SkillManager.nextCastIsFree = false;
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            return list;
        }

      
    }
}