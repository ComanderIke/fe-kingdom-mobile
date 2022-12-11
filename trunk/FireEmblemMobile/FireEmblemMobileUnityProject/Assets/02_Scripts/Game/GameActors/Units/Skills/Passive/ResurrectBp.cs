using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Resurrect", fileName = "Resurrect")]
    public class ResurrectBp : PassiveSkillBp
    {
        [SerializeField] float hpRegainPercentage;
        public override Skill Create()
        {
            return new Ressurect(Name, Description, Icon, AnimationObject,Cooldown,Tier,UpgradeDescriptions, hpRegainPercentage);
        }
    }
}