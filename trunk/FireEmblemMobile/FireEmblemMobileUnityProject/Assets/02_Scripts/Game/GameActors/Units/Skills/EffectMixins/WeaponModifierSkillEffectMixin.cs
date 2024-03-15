using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/WeaponModifier", fileName = "WeaponModifierSkillEffect")]
    public class WeaponModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        private float dexScaling;
        private float strScaling;
        public override List<EffectDescription> GetEffectDescription(Unit caster, int level)
        {
            var list = new List<EffectDescription>();
            if (strScaling!=0)
            {
                list.Add(new EffectDescription("STR Scaling: ", dexScaling*100+"%", strScaling*100+"%"));
            }
            if (dexScaling!=0)
            {
                list.Add(new EffectDescription("DEX Scaling: ", dexScaling*100+"%", dexScaling*100+"%"));
            }
            
            return list;
        }

        public override void Activate(Unit user, int level)
        {
            // user.equippedWeapon.SetDexScaling(dexScaling);
            // user.equippedWeapon.SetStrScaling(strScaling);
        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            // user.equippedWeapon.SetDexScaling(0);
            // user.equippedWeapon.SetStrScaling(1);
        }
    }
}