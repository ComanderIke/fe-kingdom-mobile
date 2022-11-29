using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/AttackBonus", fileName = "AttackBonusSkill")]
    public class AttackBonusSkillBp : PassiveSkillBp
    {
        [SerializeField] private int attackBonus = 3;

        public override Skill Create()
        {
            return new AttackBonusSkill(Name, Description, Icon, AnimationObject,Cooldown,UpgradeDescriptions, attackBonus);
        }
    }
}