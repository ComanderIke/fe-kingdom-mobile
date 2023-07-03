using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/GrowthSkill", fileName = "GrowthSkill")]
    public class GrowthSkillBp : PassiveSkillBp
    {
        [SerializeField] private Attributes growths;
        public override Skill Create()
        {
            return new GrowthsSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, growths);
        }
    }
}