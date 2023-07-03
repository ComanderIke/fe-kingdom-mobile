using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/ExtraHealingSkill", fileName = "ExtraHealingSkill")]
    public class OnHealingReceivedSkillBp : PassiveSkillBp
    {
        [SerializeField] private float healMult = 1.2f;
        public override Skill Create()
        {
            return new OnHealingReceivedSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, healMult);
        }
    }
}