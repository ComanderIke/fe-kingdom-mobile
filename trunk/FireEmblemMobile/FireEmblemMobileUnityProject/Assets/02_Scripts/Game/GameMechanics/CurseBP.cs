using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Curse", fileName = "Curse")]
    public class CurseBP : SkillBp
    {
        public override Skill Create()
        {
            return new Curse(Name, Description, Icon, Tier);
        }

    }
}