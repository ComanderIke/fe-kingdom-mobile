using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/ExpSkill", fileName = "ExpSkill")]
    public class ExpSkillBp : PassiveSkillBp
    {
        [SerializeField] private float expMul = 1.2f;
        public override Skill Create()
        {
            return new ExpSkill();
        }
    }
}