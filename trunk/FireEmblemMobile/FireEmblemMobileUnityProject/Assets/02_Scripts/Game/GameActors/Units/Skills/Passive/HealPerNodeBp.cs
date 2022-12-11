using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/HealPerNode", fileName = "HealSkill")]
    public class HealPerNodeBp : PassiveSkillBp
    {
        [SerializeField] private int hpRestored;
        public override Skill Create()
        {
            return new HealPerNode(Name, Description, Icon, AnimationObject,Cooldown,Tier, UpgradeDescriptions, hpRestored);
        }
    }
}