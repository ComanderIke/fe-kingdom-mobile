using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.GameMechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/ApplyCurse", fileName = "ApplyCurseSkillEffect")]
    public class ApplyCurseSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public GameObject effect;
        public CurseBP CurseBp;
        public override void Activate(Unit target,Unit caster, int level)
        {
            Debug.Log("ACTIVATE APPLY BUFF EFFECT MIXIN");
            if (effect != null)
                GameObject.Instantiate(effect, target.GameTransformManager.GetCenterPosition(),
                    Quaternion.identity);
            target.ReceiveCurse((Curse)CurseBp.Create(), caster.Stats.CombinedAttributes().FAITH);
        }

        public override void Deactivate(Unit target, Unit caster, int skillLevel)
        {
            
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            // list.Add(new EffectDescription("Applies: "+CurseBp.Name, "",""));
       
            return list;
        }
    }
}