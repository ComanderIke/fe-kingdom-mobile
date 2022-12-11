using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnDebuff", fileName = "OnDebuff")]
    public class DebuffProtectionEffectBp:PassiveSkillBp
    {
        [SerializeField] float procChance;
        
        public override Skill Create()
        {
            return new OnDebuff(Name, Description, Icon, AnimationObject, Cooldown, Tier,UpgradeDescriptions, procChance);
        }
    }
}