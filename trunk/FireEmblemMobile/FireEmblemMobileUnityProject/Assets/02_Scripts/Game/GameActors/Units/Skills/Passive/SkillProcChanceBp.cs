using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/SkillProcChance", fileName = "SkillProcChance")]
    public class SkillProcChanceBp : PassiveSkillBp
    {
        [SerializeField] float procChance;
        public override Skill Create()
        {
            return new SkillActivation(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, procChance);
        }
    }
}