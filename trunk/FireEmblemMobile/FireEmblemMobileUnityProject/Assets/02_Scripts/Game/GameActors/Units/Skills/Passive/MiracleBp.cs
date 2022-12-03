using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Miracle", fileName = "Miracle")]
    public class MiracleBp : PassiveSkillBp
    {
        [SerializeField] float procChance;
        public override Skill Create()
        {
            return new Miracle(Name, Description, Icon, AnimationObject,Cooldown,UpgradeDescriptions, procChance);
        }
    }
}