using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Blessing", fileName = "Blessing")]
    public class BlessingBP : SkillBp
    {
        public override Skill Create()
        {
            return new Blessing(Name, Description, Icon, Tier, passiveMixins, activeMixin);
        }

    }
}